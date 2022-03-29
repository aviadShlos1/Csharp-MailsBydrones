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
    /// Interaction logic for ParcelsListWindow.xaml
    /// </summary>
    public partial class ParcelsListWindow : Window
    {
        private BlApi.IBL blAccess;
        public ObservableCollection<ParcelToList> myParcelsPl = new();
         

        /// <summary>
        /// ctor for initialize the observable list
        /// </summary>
        /// <param name="blAccessTemp"> bl access </param>
        public ParcelsListWindow(BlApi.IBL blAccessTemp)
        {
            InitializeComponent();
            blAccess = blAccessTemp;
            blAccess.GetParcelsBl().ToList().ForEach(x => myParcelsPl.Add(x));
            ParcelsListView.ItemsSource = myParcelsPl;
            myParcelsPl.CollectionChanged += MyParcelsPl_CollectionChanged;
            ParcelsListView.ItemsSource = myParcelsPl;
            StatusSelector.ItemsSource = Enum.GetValues(typeof(ParcelStatus));
            PrioritySelector.ItemsSource = Enum.GetValues(typeof(PrioritiesBL));
        }

        /// <summary>
        /// function that update the observable for every change which is done to the observable list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyParcelsPl_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ParcelsListView.ItemsSource = myParcelsPl;
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

        /// <summary>
        /// A selection event, the user will choose between the status categories 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectionOptions();
        }
        /// <summary>
        /// A selection event, the user will choose between the priority categories 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrioritySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectionOptions();
        }
        /// <summary>
        /// A button click event, add parcel window will be opened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddParcelButton_Click(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(blAccess, this).Show();
        }
        /// <summary>
        /// double click event which gets the user to the update actions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParcelsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelToList temp = (ParcelToList)ParcelsListView.SelectedItem;
            new ParcelWindow(blAccess, temp.Id, this).Show();
        }
        /// <summary>
        /// A reset button click event, the selection will be cleared
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            StatusSelector.SelectedItem = null;
            PrioritySelector.SelectedItem = null;
            selectionOptions();
            SenderRbtn.IsEnabled = true ;
            RecieveRbtn.IsEnabled = true; 
        }
        /// <summary>
        /// A button click event, this window will be closed and the lists display will be shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClosingButton_Click(object sender, RoutedEventArgs e)
        {
            new ListsDisplayWindow().Show();
            this.Close();
        }
        /// <summary>
        /// radio button event which groups the list by the sender name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SenderButton_Click(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ParcelsListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("SenderName");
            view.GroupDescriptions.Add(groupDescription);
            SenderRbtn.IsEnabled = false;
        }
        /// <summary>
        /// radio button event which groups the list by the reciever name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecieverButton_Click(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ParcelsListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("RecieverName");
            view.GroupDescriptions.Add(groupDescription);
            RecieveRbtn.IsEnabled = false;
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
