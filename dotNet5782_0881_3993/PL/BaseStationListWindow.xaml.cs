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
using System.Collections.ObjectModel;
using System.Windows.Shapes;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DronesListWindow.xaml
    /// </summary>
    public partial class BaseStationListWindow : Window
    {
        private string[] chargeStatus = { "Free", "Full" };
        private BlApi.IBL blAccess;
        public ObservableCollection<BaseStationToList> myBaseStatiobnsPl;
        public BaseStationListWindow(BlApi.IBL blAccessTemp)
        {
            InitializeComponent();
            blAccess = blAccessTemp;
            myBaseStatiobnsPl = new ObservableCollection<BaseStationToList>(blAccess.GetBaseStationsBl());
            BaseStationListView.DataContext = myBaseStatiobnsPl;
            FreeChargeSlotsSelector.ItemsSource = chargeStatus;
        }
        /// <summary>
        /// Bonus : Auxiliary method that taking into consideration all the selection options 
        /// </summary>
        public void selectionOptions()
        {
            if (SlotsAmountSelector.Text == "" && FreeChargeSlotsSelector.SelectedItem == null) //the user select none
            {
                BaseStationListView.ItemsSource = blAccess.GetBaseStationsBl();
            }
            else if (SlotsAmountSelector.Text != "" && FreeChargeSlotsSelector.SelectedItem == null)
            {
                BaseStationListView.ItemsSource = blAccess.GetBaseStationsBl().Where(x => x.FreeChargeSlots == int.Parse(SlotsAmountSelector.Text));
            }
            else if (SlotsAmountSelector.Text == "" && (string)FreeChargeSlotsSelector.SelectedItem == "Free")
            {
                BaseStationListView.ItemsSource = blAccess.GetBaseStationsBl().Where(x => x.FreeChargeSlots > 0);
            }
            else if (SlotsAmountSelector.Text != "" && (string)FreeChargeSlotsSelector.SelectedItem == "Free")
            {
                BaseStationListView.ItemsSource = blAccess.GetBaseStationsBl().Where(x => x.FreeChargeSlots == int.Parse(SlotsAmountSelector.Text));
            }
            else if (SlotsAmountSelector.Text == "" && (string)FreeChargeSlotsSelector.SelectedItem == "Full")
            {
                BaseStationListView.ItemsSource = blAccess.GetBaseStationsBl().Where(x => x.FreeChargeSlots == 0);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("invalid data, please select again");
                if (result==MessageBoxResult.OK)
                {
                    new BaseStationListWindow(blAccess);
                }
            }
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
            SlotsAmountSelector.Text = "";
            FreeChargeSlotsSelector.SelectedItem = null;
            selectionOptions();
        }
        private void BaseStationListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BaseStationToList temp = (BaseStationToList)BaseStationListView.SelectedItem;
            new BaseStationWindow(blAccess, temp.Id, this).Show();
        }
        private void EnterClicked(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Return)
            {
                selectionOptions();
                e.Handled = true;
            }
        }
        private void FreeChargeSlotsSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectionOptions();
        }

        private void ChargeSlotsButton_Click(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(BaseStationListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("FreeChargeSlots");
            view.GroupDescriptions.Add(groupDescription);
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
