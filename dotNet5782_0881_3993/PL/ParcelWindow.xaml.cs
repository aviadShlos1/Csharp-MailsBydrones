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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // adding the new parcel details
                AssignCustomerToParcel myAssignSenderToParcel = new() { Id = (int)SenderIdTbx.SelectedItem };
                AssignCustomerToParcel myAssignRecieverToParcel = new() { Id = (int)TargetIdTbx.SelectedItem };
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
            if (MyParcel.AssignningTime != null && MyParcel.SupplyingTime == null) 
            {
                AssignTbx.Text = MyParcel.DroneAssignToParcel.ToString();
            }
        }
        private void DeleteParcelButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void CloseButton(object sender, RoutedEventArgs e)
        {
            this.Close();
            localParcelsListWindow.selectionOptions();
        }
        #endregion update

        
        private void SenderTbx_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            CustomersWindow customersWindow = new(blAccess);            
            new CustomerWindow(blAccess, MyParcel.Sender.Id, customersWindow).Show();         
        }

        private void RecieverTbx_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            CustomersWindow customersWindow = new(blAccess);
            //AssignCustomerToParcel tempReciever = (AssignCustomerToParcel)RecieverTbx.Text;
            new CustomerWindow(blAccess, MyParcel.Reciever.Id, customersWindow).Show();
        }
    }
}
