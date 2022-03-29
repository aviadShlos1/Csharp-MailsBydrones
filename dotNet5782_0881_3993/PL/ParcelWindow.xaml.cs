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
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        private BlApi.IBL blAccess;
        private ParcelsListWindow localParcelsListWindow;

        #region add
        /// <summary>
        /// Ctor for add option
        /// </summary>
        /// <param name="blAccessTemp">The access parameter to the bl </param>
        /// <param name="dronesListTemp">Presents the parcel window </param>
        public ParcelWindow(BlApi.IBL blAccessTemp, ParcelsListWindow parcelsListTemp)
        {
            InitializeComponent();
            AddOption.Visibility = Visibility.Visible; // the add option will be shown
            blAccess = blAccessTemp;
            localParcelsListWindow = parcelsListTemp;
            WeightTbx.ItemsSource = Enum.GetValues(typeof(WeightCategoriesBL));
            PriorityTbx.ItemsSource = Enum.GetValues(typeof(PrioritiesBL));
            IEnumerable<int> customersId = blAccess.GetCustomersBl().Select(x => x.Id);
            SenderIdTbx.ItemsSource = customersId;
            TargetIdTbx.ItemsSource = customersId;
        }
        // <summary>
        /// A click buttun event. When the user will click this button, the add options window will be closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseAddButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// An add click buttun event. When the user will click this button, the parcel will be added
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // adding the new parcel details
                AssignCustomerToParcel myAssignSenderToParcel = new() { CustId = (int)SenderIdTbx.SelectedItem };
                AssignCustomerToParcel myAssignRecieverToParcel = new() { CustId = (int)TargetIdTbx.SelectedItem };
                ParcelBl newParcel = new ParcelBl
                {
                    Sender = myAssignSenderToParcel,
                    Reciever = myAssignRecieverToParcel,
                    ParcelWeight = (WeightCategoriesBL)WeightTbx.SelectedItem,
                    Priority = (PrioritiesBL)PriorityTbx.SelectedItem
                };
                blAccess.AddParcel(newParcel);

                MessageBoxResult result = MessageBox.Show("The operation was done successfully");
                if (result == MessageBoxResult.OK)
                {
                    this.Close();
                    localParcelsListWindow.selectionOptions();
                }
            }
            catch (AlreadyExistException)
            {
                MessageBox.Show("This parcel is already exists");
                SenderIdTbx.BorderBrush = Brushes.Red;
                PriorityTbx.Background = Brushes.Red;
            }
        }
        #endregion add

        #region update
        public ParcelBl MyParcel;

        // Ctor for update options
        public ParcelWindow(BlApi.IBL blAccessTemp, int parcelId, ParcelsListWindow parcelListTemp)
        {
            InitializeComponent();
            this.Topmost = true;
            UpdateOptions.Visibility = Visibility.Visible; //The update options window will be shown
            localParcelsListWindow = parcelListTemp;
            blAccess = blAccessTemp;
            MyParcel = blAccess.GetSingleParcel(parcelId);
            UpdateOptions.DataContext = MyParcel;
            SenderTbx.Text = MyParcel.Sender.ToString();
            RecieverTbx.Text = MyParcel.Reciever.ToString();
            if (MyParcel.AssignningTime==null)
            {
                TrashIcon.Visibility = Visibility.Visible;
                DeleteParcelButton.Visibility = Visibility.Visible;
            }
            if (MyParcel.AssignningTime != null && MyParcel.PickingUpTime == null)
            {
                //AssignTbx.Text = MyParcel.DroneAssignToParcel.ToString();
                PickUpParcelButton.Visibility = Visibility.Visible;
            }
            if (MyParcel.PickingUpTime != null && MyParcel.SupplyingTime == null)
            { 
                //AssignTbx.Text = MyParcel.DroneAssignToParcel.ToString();
                SupplyParcelButton.Visibility = Visibility.Visible;
            }
        }
        /// <summary>
        /// click button for delete the selected item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteParcelButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("You are going to delete the parcel, sure about it?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Yes:

                    blAccess.RemoveParcel(MyParcel);
                    int index = localParcelsListWindow.myParcelsPl.ToList().FindIndex(x=>x.Id==MyParcel.ParcelId);// we go to the index to delete from the observer 
                    localParcelsListWindow.myParcelsPl.RemoveAt(index);
                    this.Close();
                    localParcelsListWindow.selectionOptions();// update the displey of the list view to observ the changes
                    break;
                case MessageBoxResult.No: // in case that the user dont want to delete he have the option to abort withot any change 
                    break;
                default:
                    break;
            }
        }
      /// <summary>
      /// click button that presents the customer who sent this parcel 
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
        private void SenderTbx_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            CustomersListWindow customersWindow = new(blAccess);
            new CustomerWindow(blAccess, MyParcel.Sender.CustId, customersWindow).Show();
        }
        /// <summary>
        /// click button that presents the customer who recieved this parcel 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecieverTbx_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            CustomersListWindow customersWindow = new(blAccess);
            //AssignCustomerToParcel tempReciever = (AssignCustomerToParcel)RecieverTbx.Text;
            new CustomerWindow(blAccess, MyParcel.Reciever.CustId, customersWindow).Show();
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
                blAccess.PickUpParcel(MyParcel.DroneAssignToParcel.DroneId);
                MessageBox.Show("The parcel picked up by the drone successfully");
                new ParcelWindow(blAccess, MyParcel.ParcelId, localParcelsListWindow).Show();
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

                blAccess.SupplyParcel(MyParcel.DroneAssignToParcel.DroneId);
                MessageBox.Show("The parcel supplied by the drone successfully");
                new ParcelWindow(blAccess, MyParcel.ParcelId, localParcelsListWindow).Show();
                Close();
            }
            catch (CannotSupplyException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion update
        /// <summary>
        /// click button that presents the drone which is assigned to this parcel 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DroneAssignTbx_MouseEnter(object sender, MouseEventArgs e)
        {
            DronesListWindow temp = new(blAccess);
            new DroneWindow(blAccess, MyParcel.DroneAssignToParcel.DroneId, temp).Show();
        }

        private void CloseUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            localParcelsListWindow.selectionOptions();
        }

       
    }
}

    

