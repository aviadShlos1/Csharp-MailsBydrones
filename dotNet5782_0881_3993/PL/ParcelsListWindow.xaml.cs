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
    /// Interaction logic for ParcelsListWindow.xaml
    /// </summary>
    public partial class ParcelsListWindow : Window
    {
        private BlApi.IBL blAccess;
        private ObservableCollection<ParcelToList> myParcelsPl = new();
        public ParcelsListWindow(BlApi.IBL blAccessTemp)
        {
            InitializeComponent();
            blAccess = blAccessTemp;
            blAccess.GetParcelsBl().ToList().ForEach(x => myParcelsPl.Add(x));
            ParcelsListView.ItemsSource = myParcelsPl;
            StatusSelector.ItemsSource = Enum.GetValues(typeof(ParcelStatus));
            PrioritySelector.ItemsSource = Enum.GetValues(typeof(PrioritiesBL));
        }

        /// <summary>
        /// Bonus : Auxiliary method that taking into consideration all the selection options 
        /// </summary>
        public void selectionOptions()
        {           
            if (PrioritySelector.SelectedItem == null && StatusSelector.SelectedItem == null) //the user select none
            {
                ParcelsListView.ItemsSource = blAccess.GetParcelsBl();
            }
            else if (PrioritySelector.SelectedItem == null) // the user selected by status
            {
                ParcelsListView.ItemsSource = blAccess.GetParcelsBl(x => x.ParcelStatus == (ParcelStatus)StatusSelector.SelectedItem);
            }
            else if (StatusSelector.SelectedItem == null)// the user selected by weight
            {
                ParcelsListView.ItemsSource = blAccess.GetParcelsBl(x => x.Priority == (PrioritiesBL)PrioritySelector.SelectedItem);
            }
            else // the user selected both by status and weight
            {
                ParcelsListView.ItemsSource = blAccess.GetParcelsBl(x => x.ParcelStatus == (ParcelStatus)StatusSelector.SelectedItem && x.Priority == (PrioritiesBL)PrioritySelector.SelectedItem);
            }
        }

     
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectionOptions();
        }

        private void PrioritySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectionOptions();
        }
        private void AddParcelButton_Click(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(blAccess, this).Show();
        }
        private void ParcelsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelToList temp = (ParcelToList)ParcelsListView.SelectedItem;
            new ParcelWindow(blAccess, temp.Id, this).Show();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            StatusSelector.SelectedItem = null;
            PrioritySelector.SelectedItem = null;
            selectionOptions();
        }
        private void ClosingButton_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }


    }
}
