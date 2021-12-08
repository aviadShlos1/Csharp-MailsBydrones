using System;
using System.Collections.Generic;
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
using IBL.BO;
namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private IBL.IBL blAccess;
        private DronesListWindow localDronesListWindow;
        private int[] BaseStationNum = new int[] { 0,1 };
        private int firstChargeStation = default;

        #region Add
        public DroneWindow(IBL.IBL blAccessTemp, DronesListWindow dronesListTemp)//C-tor for add option
        {
            InitializeComponent();
            AddOption.Visibility = Visibility.Visible;
            blAccess = blAccessTemp;
            localDronesListWindow = dronesListTemp;
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategoriesBL));
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatusesBL));
            BaseStationIdSelector.ItemsSource = BaseStationNum;
        }
        private void WeightComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WeightSelector.SelectedItem = Enum.GetValues(typeof(WeightCategoriesBL));
        }
        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatusSelector.SelectedItem = Enum.GetValues(typeof(DroneStatusesBL));
        }
        private void BaseStationIdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BaseStationIdSelector.SelectedItem = BaseStationNum;
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            localDronesListWindow.selectionOptions();
            this.Close();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DroneToList newDrone = new DroneToList
                {
                    DroneId = int.Parse(IdTbx.Text),
                    Model = ModelTbx.Text,
                    DroneWeight = (WeightCategoriesBL)WeightSelector.SelectedItem,
                };
                firstChargeStation = (int)BaseStationIdSelector.SelectedItem;
                blAccess.AddDrone(newDrone, firstChargeStation);
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
        public DroneWindow(IBL.IBL blAccessTemp, int droneId, DronesListWindow dronesListTemp)//C-tor for update options
        {
            InitializeComponent();
            UpdateOptions.Visibility = Visibility.Visible;
            localDronesListWindow = dronesListTemp;
            blAccess = blAccessTemp;
            MyDrone = blAccess.GetSingleDrone(droneId);
            UpdateOptions.DataContext = MyDrone;
            DroneLocation.Text = MyDrone.DroneLocation.ToString();
            //ParcelInShipment.Text = MyDrone.ParcelInShip.ToString();
            
            switch ((DroneStatusesBL)MyDrone.DroneStatus) // checking the drone status, correspondingly enables the operations
            {
                case DroneStatusesBL.Available:
                    DroneToChargeButton.Visibility = Visibility.Visible;
                    AssignParcelToDroneButton.Visibility = Visibility.Visible;
                    break;

                case DroneStatusesBL.Maintaince:
                    ReleaseFromChargeButton.Visibility = Visibility.Visible;
                    //BReleaseDrone.IsEnabled = false;
                    //TimeChoose.Visibility = Visibility.Visible;
                    break;

                case DroneStatusesBL.Shipment:
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
     
        private void NameUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            blAccess.UpdateDroneName(MyDrone.DroneId, DroneModelTbx.Text);
            MessageBox.Show("Your update was done successfully");
            new DroneWindow(blAccess, MyDrone.DroneId, localDronesListWindow).Show(); 
            Close();
        }

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
        private void CloseUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            localDronesListWindow.selectionOptions();
        }
        #endregion
    }
}
