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
using System.Collections.ObjectModel;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for BaseStationListWindow.xaml
    /// </summary>
    public partial class BaseStationListWindow : Window
    {
        private BlApi.IBL blAccess;
        public ObservableCollection<BaseStationToList> myBaseStatiobnsPl;
        private string[] chargeStatus = { "Free", "Full" };

        /// <summary>
        /// ctor for initialize the observable list
        /// </summary>
        /// <param name="blAccessTemp"> bl access </param>
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
        /// A button click event, add base station window will be opened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddBaseStationButton_Click(object sender, RoutedEventArgs e)
        {
            new BaseStationWindow(blAccess, this).Show();
        }
        /// <summary>
        /// A button click event, this window will be closed and the lists display will be shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClosingWindowButton_Click(object sender, RoutedEventArgs e)
        {
            new ListsDisplayWindow().Show();
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
            ChargeCbx.IsEnabled = true;
        }
      
        private void FreeChargeSlotsSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectionOptions();
        }
       /// <summary>
       /// double click event which gets the user to the update actions
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void BaseStationListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BaseStationToList temp = (BaseStationToList)BaseStationListView.SelectedItem;
            new BaseStationWindow(blAccess, temp.Id, this).Show();
        }
        /// <summary>
        /// event for enter key . When the user will push the enter key the list will be selected by the free charge slots number input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnterClicked(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                selectionOptions();
                e.Handled = true;
            }
        }
        /// <summary>
        /// radio button event which groups the list by free charge slots amount
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChargeSlotsRadioBut(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(BaseStationListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("FreeChargeSlots");
            view.GroupDescriptions.Add(groupDescription);
            ChargeCbx.IsEnabled = false;
        }
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
