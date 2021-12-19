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
    enum ChargeSlotStatus { Free, Full}
    /// <summary>
    /// Interaction logic for DronesListWindow.xaml
    /// </summary>
    public partial class BaseStationListWindow : Window
    {
        private BlApi.IBL blAccess;
        public BaseStationListWindow(BlApi.IBL blAccessTemp)
        {
            InitializeComponent();
            blAccess = blAccessTemp;
            BaseStationListView.ItemsSource = blAccess.GetBaseStationsBl();
            FreeChargeSlotsSelector.ItemsSource = Enum.GetValues(typeof(ChargeSlotStatus));
        }
        /// <summary>
        /// Bonus : Auxiliary method that taking into consideration all the selection options 
        /// </summary>
        public void selectionOptions()
        {
            if (FreeChargeSlotsSelector.SelectedItem==null) //the user select none
            {
                BaseStationListView.ItemsSource = blAccess.GetBaseStationsBl();
            }
            else if (FreeChargeSlotsSelector.SelectedItem != null)
            {
                BaseStationListView.ItemsSource = blAccess.GetBaseStationsBl(x => x.FreeChargeSlots == (int)FreeChargeSlotsSelector.SelectedItem) ;
            }
        }

        /// <summary>//
        /// A selection event, the user will choose between the status categories 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_FreeChargeSlotsSelection(object sender, SelectionChangedEventArgs e)//
        {
            selectionOptions();
        }


        /// <summary>
        /// A button click event, the add drone window will be opened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddBaseStationButton_Click(object sender, RoutedEventArgs e)
        {
            new BaseStationWindow(blAccess, this).Show();
        }
        /// <summary>
        /// A button click event, the add drone window will be opened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClosingWindowButton_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
        /// <summary>
        /// A reset button click event, the selection will be cleared
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            FreeChargeSlotsSelector.SelectedItem = null;
            //DroneListView.ItemsSource = droneToLists;
            //selectionOptions();
        }
        /// <summary>
        /// A double click event. The user will click double click on the wanted drone, in that he will be able to do some action with it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void BaseStationList_DoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    BaseStationToList temp = (BaseStationToList)BaseStationListView.SelectedItem;
        //    new BaseStationWindow(blAccess, temp.Id, this).Show();
        //}
    }
}
