//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil3
//brief: In this program we built the presentation layer
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BO;
namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        //private ObservableCollection<DroneToList> myDronesPl = new ObservableCollection<DroneToList>();
        private BlApi.IBL blAccess;
        private DronesListWindow localDronesListWindow;
        //private int[] BaseStationNum = new int[] { 0,1 }; //An array which includes the base stations id
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
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            localDronesListWindow.selectionOptions(); // Call the selection function to present the selected list according to the user selection
            this.Close();
        }
        /// <summary>
        /// An add click buttun event. When the user will click this button, the add options window will be opened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // adding the new drone details

                DroneToList newDrone = new DroneToList
                {
                    DroneId = int.Parse(IdTbx.Text),
                    Model = ModelTbx.Text,
                    DroneWeight = (WeightCategoriesBL)WeightSelector.SelectedItem,
                };
                firstChargeStation = (int)BaseStationIdSelector.SelectedItem;
                blAccess.AddDrone(newDrone, firstChargeStation);
                //myDronesPl.Add(newDrone);
                
                MessageBoxResult result = MessageBox.Show("The operation was done successfully");
                if (result == MessageBoxResult.OK)
                {
                    this.Close();
                    localDronesListWindow.selectionOptions();
                }
            }
            catch (AlreadyExistException)
            {
                MessageBox.Show("This drone is already exists");
                IdTbx.BorderBrush = Brushes.Red;
                IdTbl.Background = Brushes.Red;
            }
        }

        #endregion

        #region Update 
        public DroneBl MyDrone;

        // Ctor for update options
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
            var textBox = new TextBox();
            blAccess.UpdateDroneName(MyDrone.DroneId, textBox.Text);
            MessageBox.Show("Your update was done successfully");
            new DroneWindow(blAccess, MyDrone.DroneId, localDronesListWindow).Show(); 
            Close();
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
        #endregion
    }
}
