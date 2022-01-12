//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Level 3
//Brief: Improve the presentation and add user interface
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using BO;
namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private BlApi.IBL blAccess;
        private DronesListWindow localDronesListWindow;
        private int firstChargeStation = default;
        public IEnumerable<int> baseId;

        #region Add
        /// <summary>
        /// Ctor for add option
        /// </summary>
        /// <param name="blAccessTemp">The access parameter to the bl </param>
        /// <param name="dronesListTemp">Presents the drones window </param>
        public DroneWindow(BlApi.IBL blAccessTemp, DronesListWindow dronesListTemp)
        {
            InitializeComponent();
            AddOption.Visibility = Visibility.Visible; // the add option will be shown
            blAccess = blAccessTemp;
            localDronesListWindow = dronesListTemp; 
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategoriesBL));
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatusesBL));
            baseId = blAccess.GetBaseStationsBl().Select(item => item.Id);
            BaseStationIdSelector.ItemsSource = baseId;
        }
        /// <summary>
        /// A selection event, the user will choose between the weight categories 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WeightComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WeightSelector.SelectedItem = Enum.GetValues(typeof(WeightCategoriesBL));
        }
        /// <summary>
        /// A selection event, the user will choose between the status categories 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatusSelector.SelectedItem = Enum.GetValues(typeof(DroneStatusesBL));
        }
        /// <summary>
        /// A selection event, the user will choose between the base stations  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseStationIdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BaseStationIdSelector.SelectedItem = baseId;
        }
        /// <summary>
        /// A click buttun event. When the user will click this button, the add options window will be closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseAddButton_Click(object sender, RoutedEventArgs e)
        {
            localDronesListWindow.selectionOptions(); // Call the selection function to present the selected list according to the user selection
            this.Close();
        }
        /// <summary>
        
        /// An add click buttun event. When the user will click this button, the drone will be added
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // adding the new drone details

                DroneToList newDrone = new()
                {
                    DroneId = int.Parse(IdTbx.Text),
                    Model = ModelTbx.Text,
                };
                firstChargeStation = (int)BaseStationIdSelector.SelectedItem;
                this.AddOption.DataContext = newDrone;
                blAccess.AddDrone(newDrone, firstChargeStation);
                
                MessageBoxResult result = MessageBox.Show("The operation was done successfully");
                if (result == MessageBoxResult.OK)
                {
                    localDronesListWindow.myDronesPl.Add(newDrone);
                    this.Close();
                    localDronesListWindow.selectionOptions();
                }
            }
            catch (AlreadyExistException)
            {
                MessageBox.Show("This drone is already exists");
                IdTbx.BorderBrush = Brushes.Red;
            }
        }

        #endregion Add

        #region Update 
        public DroneBl MyDrone;

        /// <summary>
        /// Ctor for update options
        /// </summary>
        /// <param name="blAccessTemp"></param>
        /// <param name="droneId">Get drone id to get the drone</param>
        /// <param name="dronesListTemp">Given in order to Enable update the drones list</param>
        public DroneWindow(BlApi.IBL blAccessTemp, int droneId, DronesListWindow dronesListTemp)
        {
            InitializeComponent();
            this.Topmost = true;
            
            UpdateOptions.Visibility = Visibility.Visible; //The update options window will be shown
            localDronesListWindow = dronesListTemp;
            blAccess = blAccessTemp;
            MyDrone = blAccess.GetSingleDrone(droneId);
            UpdateOptions.DataContext = MyDrone;
            DroneLoc.Text = MyDrone.DroneLocation.ToString();
            
            switch ((DroneStatusesBL)MyDrone.DroneStatus) // checking the drone status, correspondingly enables the operations
            {
                case DroneStatusesBL.Available:
                    DroneToChargeButton.Visibility = Visibility.Visible;
                    AssignParcelToDroneButton.Visibility = Visibility.Visible;
                    break;

                case DroneStatusesBL.Maintaince:
                    ReleaseFromChargeButton.Visibility = Visibility.Visible;
                    break;

                case DroneStatusesBL.Shipment:
                    ParcelInShipmentTbl.Visibility = Visibility.Visible;
                    ParcelInShipmentTbx.Visibility = Visibility.Visible;
                    ParcelInShipmentTbx.Text = MyDrone.ParcelInShip.ToString();
                    if (MyDrone.ParcelInShip.ShippingOnTheSupplyWay==true)
                    {
                        SupplyParcelButton.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        PickUpParcelButton.Visibility = Visibility.Visible;
                    }
                    break;

                default:
                    break;
            }
        }
         /// <summary>
         /// Button click event, which will enable the user to update the drone name
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
        private void NameUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            blAccess.UpdateDroneName(MyDrone.DroneId, DroneModelTbx.Text);
            MessageBox.Show("Your update was done successfully");
            new DroneWindow(blAccess, MyDrone.DroneId, localDronesListWindow).Show(); 
            this.Close();
        }
        /// <summary>
        /// Button click event, which will enable the user to send the drone to charge
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>    
        private void DroneToChargeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                blAccess.DroneToCharge(MyDrone.DroneId);
                MessageBox.Show("The drone sent to charge successfully");
                new DroneWindow(blAccess, MyDrone.DroneId, localDronesListWindow).Show();
                Close();
            }
            catch (CannotGoToChargeException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// Button click event, which will enable the user to release the drone from charge
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReleaseFromChargeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {               
                blAccess.ReleaseDroneCharge(MyDrone.DroneId);
                MessageBox.Show("The drone released from charge successfully");
                new DroneWindow(blAccess, MyDrone.DroneId, localDronesListWindow).Show();
                Close();
            }
            catch (CannotReleaseFromChargeException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// Button click event, which will enable the user to assign bewteen drone to parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssignParcelToDroneButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                blAccess.AssignParcelToDrone(MyDrone.DroneId);
                MessageBox.Show("The drone assigned to parcel successfully");
                new DroneWindow(blAccess, MyDrone.DroneId, localDronesListWindow).Show();
                Close();
            }
            catch (CannotAssignDroneToParcelException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// Button click event, which will enable the user to send the dron to pick up a parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PickUpParcelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                blAccess.PickUpParcel(MyDrone.DroneId);
                MessageBox.Show("The drone picked up the parcel successfully");
                new DroneWindow(blAccess, MyDrone.DroneId, localDronesListWindow).Show();
                Close();
            }
            catch (CannotPickUpException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// Button click event, which will enable the user to send the dron to supply a parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SupplyParcelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                blAccess.SupplyParcel(MyDrone.DroneId);
                MessageBox.Show("The drone supplied the parcel successfully");
                new DroneWindow(blAccess, MyDrone.DroneId, localDronesListWindow).Show();
                Close();
            }
            catch (CannotSupplyException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        /// <summary>
        /// Button click event, which will enable the user to close the update window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            localDronesListWindow.selectionOptions();
        }
        /// <summary>
        /// Double Click event that open the window of the assignning parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParcelInShipmentTbx_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(MyDrone.ParcelInShip!=null)
            {
                ParcelsListWindow TempParcelsListWindow = new(blAccess);
                new ParcelWindow(blAccess, MyDrone.ParcelInShip.Id, TempParcelsListWindow).Show();
            }          
        }
        #endregion

        #region Simulator
        internal BackgroundWorker DroneSimulator;
        private void Simulator()
        {
            DroneSimulator = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            DroneSimulator.DoWork += DroneSimulator_DoWork; ; //Operation function.
            DroneSimulator.ProgressChanged += DroneSimulator_ProgressChanged; // change reporter
            DroneSimulator.RunWorkerCompleted += DroneSimulator_RunWorkerCompleted; //  thread complete
        }

        private void DroneSimulator_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private int ParcelInShipId;
        private int SenderCustomerId;
        private int ReceiverCustomerId;
        private void DroneSimulator_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //to update conect the binding to set the value of my drone to the proprtis.
            MyDrone = blAccess.GetSingleDrone(MyDrone.DroneId);
            DataContext = MyDrone;

            localDronesListWindow.selectionOptions(); //update the List of drones.

            CustomersListWindow myCustomers = new(blAccess);
            ParcelsListWindow myParcels = new(blAccess);
            BaseStationListWindow myBase = new(blAccess);
            
            // to find the index when the fanc need to find in the observer collaction and update.
            int ParcelIndex;
            int SenderCustomerIndex;
            int RecieverCustomerIndex;

            //switch betwen drone status and according to that update the display.
            switch (MyDrone.DroneStatus)
            {
                case DroneStatusesBL.Available:
                    if (MyDrone.ParcelInShip.Id<0) //the drone is free cuse he just done (we know that becuse the grid is opend) it is affter deliverd.
                    {

                        //update the parcels list
                        ParcelIndex = myParcels.myParcelsPl.IndexOf(myParcels.myParcelsPl.First(x => x.Id == ParcelInShipId));
                        myParcels.myParcelsPl[ParcelIndex] = blAccess.GetParcelsBl().First(x => x.Id == ParcelInShipId);

                        //update spasice customer in the Customer list (sender)
                        SenderCustomerIndex = myCustomers.myCustomerPl.IndexOf(myCustomers.myCustomerPl.First(x => x.Id == SenderCustomerId));
                        myCustomers.myCustomerPl[SenderCustomerIndex] = blAccess.GetCustomersBl().First(x => x.Id == SenderCustomerId);

                        //update the reciver
                        RecieverCustomerIndex = myCustomers.myCustomerPl.IndexOf(myCustomers.myCustomerPl.First(x => x.Id == ReceiverCustomerId));
                        myCustomers.myCustomerPl[RecieverCustomerIndex] = blAccess.GetCustomersBl().First(x => x.Id == ReceiverCustomerId);


                    }
                    else //the drone is in a free state that has come out of charge and not like before (not affter deliver).
                    {
                        myBase.myBaseStatiobnsPl.Clear();
                        myBase.myBaseStatiobnsPl = new ObservableCollection<BaseStationToList>(blAccess.GetBaseStationsBl());
                    }

                    break;

                case DroneStatusesBL.Maintaince:
                    myBase.myBaseStatiobnsPl.Clear();
                    myBase.myBaseStatiobnsPl = new ObservableCollection<BaseStationToList>(blAccess.GetBaseStationsBl());
                    break;

                case DroneStatusesBL.Shipment:
                    ParcelInShipId = MyDrone.ParcelInShip.Id;
                    SenderCustomerId = MyDrone.ParcelInShip.Sender.CustId;
                    ReceiverCustomerId = MyDrone.ParcelInShip.Reciever.CustId;

                    if (blAccess.GetSingleParcel(MyDrone.ParcelInShip.Id).PickingUpTime == null)
                    {
                        ParcelInShipId = MyDrone.ParcelInShip.Id;
                        SenderCustomerId = MyDrone.ParcelInShip.Sender.CustId;
                        ReceiverCustomerId = MyDrone.ParcelInShip.Reciever.CustId;

                        //update list of parcels
                        ParcelIndex = myParcels.myParcelsPl.IndexOf(myParcels.myParcelsPl.First(x => x.Id == MyDrone.ParcelInShip.Id));
                        myParcels.myParcelsPl[ParcelIndex] = blAccess.GetParcelsBl().First(x => x.Id == MyDrone.ParcelInShip.Id);

                     
                    }
                    else if (blAccess.GetSingleParcel(MyDrone.ParcelInShip.Id).SupplyingTime == null)
                    {
                        //update the parcels list
                        ParcelIndex = myParcels.myParcelsPl.IndexOf(myParcels.myParcelsPl.First(x => x.Id == MyDrone.ParcelInShip.Id));
                        myParcels.myParcelsPl[ParcelIndex] = blAccess.GetParcelsBl().First(x => x.Id == MyDrone.ParcelInShip.Id);
                        //update spasice customer in the Customer list (sender)
                        SenderCustomerIndex = myCustomers.myCustomerPl.IndexOf(myCustomers.myCustomerPl.First(x => x.Id == MyDrone.ParcelInShip.Sender.CustId));
                        myCustomers.myCustomerPl[SenderCustomerIndex] = blAccess.GetCustomersBl().First(x => x.Id == MyDrone.ParcelInShip.Sender.CustId);
                        //update the reciver
                        RecieverCustomerIndex = myCustomers.myCustomerPl.IndexOf(myCustomers.myCustomerPl.First(x => x.Id == MyDrone.ParcelInShip.Reciever.CustId));
                        myCustomers.myCustomerPl[RecieverCustomerIndex] = blAccess.GetCustomersBl().First(x => x.Id == MyDrone.ParcelInShip.Reciever.CustId);
                    }
                    break;

                default:
                    break;
            }

            //battery colors.
            if (MyDrone.BatteryPercent < 50)
            {
                if (MyDrone.BatteryPercent > 20)
                {
                    DroneBattery.Foreground = Brushes.YellowGreen;
                }
                else
                {
                    DroneBattery.Foreground = Brushes.Red;
                }
            }
            else //MyDrone.BatteryPercent > 50
            {
                DroneBattery.Foreground = Brushes.LimeGreen;

            }
        }

        private void DroneSimulator_DoWork(object sender, DoWorkEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void ReportProgressInSimultor()
        {
            DroneSimulator.ReportProgress(0);
        }

        public bool IsTimeRun()
        {
            return DroneSimulator.CancellationPending;
        }
        private void AutomaticBut_Click(object sender, RoutedEventArgs e)
        {
            Simulator(); //call to function which creates the process.

            DroneSimulator.RunWorkerAsync(); //Start running the process.

            //Hiding the other buttons in the background.
            DroneToChargeButton.Visibility = Visibility.Hidden;
            ReleaseFromChargeButton.Visibility = Visibility.Hidden;
            AssignParcelToDroneButton.Visibility = Visibility.Hidden;
            PickUpParcelButton.Visibility = Visibility.Hidden;
            SupplyParcelButton.Visibility = Visibility.Hidden;

            //Hiding the automatic process button and opening a manually process button.
            AutomaticBut.Visibility = Visibility.Hidden;
            ManualBut.Visibility = Visibility.Visible;

            ModelTbx.IsEnabled = false; //to prevent model changing
        }
       
        #endregion Simulator 

        private void ManualBut_Click(object sender, RoutedEventArgs e)
        {
            DroneSimulator.CancelAsync();
            //while(MyDrone.Statuses!=DroneStatuses.free)
            //{ }
            //isTimeRun = false;
            AutomaticBut.Visibility = Visibility.Visible;
            ManualBut.Visibility = Visibility.Hidden;

            DroneToChargeButton.Visibility = Visibility.Visible;
            AssignParcelToDroneButton.Visibility = Visibility.Visible;            
            ModelTbx.IsEnabled = true;
        }
    }

}
