//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Level 3
//Brief: Improve the presentation and add user interface
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
using BO;
namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class BaseStationWindow : Window
    {
        private BlApi.IBL blAccess;
        private BaseStationListWindow localBaseStationListWindow;
        

        #region add
        /// <summary>
        /// Ctor for add option
        /// </summary>
        /// <param name="blAccessTemp">The access parameter to the bl </param>
        /// <param name="BaseStationsListTemp">Presents the baseStations window </param>
        public BaseStationWindow(BlApi.IBL blAccessTemp, BaseStationListWindow BaseStationsListTemp)
        {
            InitializeComponent();
            BaseStaionAddOption.Visibility = Visibility.Visible; // the add option will be shown 
            blAccess = blAccessTemp;
            localBaseStationListWindow = BaseStationsListTemp;
        }
        /// <summary>
        /// A click buttun event. When the user will click this button, the add options window will be closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            localBaseStationListWindow.selectionOptions(); // Call the selection function to present the selected list according to the user selection
            this.Close();
        }
        /// An add click buttun event. When the user will click this button, the base station will be added
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddBaseStationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // adding the new base station details

                BaseStationBl newBaseStation = new BaseStationBl()
                {
                    Id = int.Parse(IdTbx.Text),
                    BaseStationName = NameTbx.Text,
                    FreeChargeSlots = int.Parse(AddFreeSlotsTbx.Text),
                    Location = new Location() { Longitude = double.Parse(LongitudeTbx.Text), Latitude = double.Parse(LatitudeTbx.Text) },
                };
                this.BaseStaionAddOption.DataContext = newBaseStation;
                blAccess.AddBaseStation(newBaseStation);
                MessageBoxResult result = MessageBox.Show("The operation was done successfully");
                if (result == MessageBoxResult.OK)
                {
                    BaseStationToList baseStationToList = blAccess.GetBaseStationsBl().Find(x => x.Id == newBaseStation.Id);
                    localBaseStationListWindow.myBaseStatiobnsPl.Add(baseStationToList);
                    this.Close();
                    localBaseStationListWindow.selectionOptions();
                }
            }
            catch (AlreadyExistException)
            {
                MessageBox.Show("This drone is already exists");
                IdTbx.BorderBrush = Brushes.Red;
                IdTbl.Background = Brushes.Red;
            }
        }
        #endregion add

        #region Update 
        public BaseStationBl MyBase;

        // Ctor for update options
        public BaseStationWindow(BlApi.IBL blAccessTemp, int baseId, BaseStationListWindow baseStationsListTemp)
        {
            InitializeComponent();
            UpdateOptions.Visibility = Visibility.Visible; //The update options window will be shown
            localBaseStationListWindow = baseStationsListTemp;
            blAccess = blAccessTemp;
            MyBase = blAccess.GetSingleBaseStation(baseId);
            UpdateOptions.DataContext = MyBase;
            BaseLocation.Text = MyBase.Location.ToString();
            DronesInChargeLbx.ItemsSource = MyBase.DronesInChargeList;
        }
        /// <summary>
        /// button which will allow updating the base station name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateNameButton_Click(object sender, RoutedEventArgs e)
        {
            blAccess.UpdateBaseStationData(MyBase.Id, NameTbx.Text, int.Parse(UpdateFreeChargeTbx.Text));
            MessageBox.Show("Your update was done successfully");
            localBaseStationListWindow.selectionOptions();
            this.Close();
        }
        /// <summary>
        /// double click event which opens the select drone window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DronesInChargeLbx_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneInCharge temp = (DroneInCharge)DronesInChargeLbx.SelectedItem;
            DronesListWindow dronesListWindow = new DronesListWindow(blAccess);
            new DroneWindow(blAccess, temp.Id, dronesListWindow).Show();      
        }

        private void CloseUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            localBaseStationListWindow.selectionOptions();
        }
        #endregion update

        /// <summary>
        /// draging the window by holding it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Drag(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}