//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil3
//brief: In this program we built the presentation layer
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
        //private int[] BaseStationNum = new int[] { 0, 1 }; //An array which includes the base stations id

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

        private void AddBaseStationButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                // adding the new drone details

                BaseStationBl newBaseStation = new BaseStationBl
                {
                    Id = int.Parse(IdTbx.Text),
                    BaseStationName = NameTbx.Text,
                    FreeChargeSlots = int.Parse(AddFreeSlotsTbx.Text),
                    Location = new Location() { Longitude = double.Parse(LongitudeTbx.Text), Latitude = double.Parse(LatitudeTbx.Text) },
                };
                blAccess.AddBaseStation(newBaseStation);

                MessageBoxResult result = MessageBox.Show("The operation was done successfully");
                if (result == MessageBoxResult.OK)
                {
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


        }
        private void UpdateNameButton_Click(object sender, RoutedEventArgs e)
        {
            blAccess.UpdateBaseStationData(MyBase.Id, NameTbx.Text, int.Parse(UpdateFreeChargeTbx.Text));
            MessageBox.Show("Your update was done successfully");
            localBaseStationListWindow.selectionOptions();
            this.Close();

        }

        private void CloseBaseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            localBaseStationListWindow.selectionOptions();
        }
        #endregion update
    }
}