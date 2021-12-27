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
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private BlApi.IBL blAccess;
        private CustomersWindow localCustomersWindow;
        #region Add
        public CustomerWindow(BlApi.IBL blAccessTemp, CustomersWindow customersListTemp)
        {
            InitializeComponent();
            AddOption.Visibility = Visibility.Visible; // the add option will be shown
            blAccess = blAccessTemp;
            localCustomersWindow = customersListTemp;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // adding the new customer details
                CustomerBL newCustomer = new CustomerBL()
                {
                    Id = int.Parse(IdTbx.Text),
                    Name = NameTbx.Text,
                    Phone = PhoneTbx.Text,
                    Location = new Location() { Latitude = double.Parse(LatitudeTbx.Text), Longitude = double.Parse(LongitudeTbx.Text) },
                };
                this.AddOption.DataContext = newCustomer;
                blAccess.AddCustomer(newCustomer);

                MessageBoxResult result = MessageBox.Show("The operation was done successfully");
                if (result == MessageBoxResult.OK)
                {
                    this.Close();
                    CustomerToList myCustomer = blAccess.GetCustomersBl().Find(x => x.Id == newCustomer.Id);
                    localCustomersWindow.myCustomerPl.Add(myCustomer);
                    localCustomersWindow.selectionOptions();
                }
            }
            catch (AlreadyExistException)
            {
                MessageBox.Show("This customer is already exists");
                IdTbx.BorderBrush = Brushes.Red;
                IdTbl.Background = Brushes.Red;
            }
        }
        private void CloseAddButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            localCustomersWindow.selectionOptions();
        }

        #endregion

        #region Update 
        public CustomerBL MyCustomer;

        // Ctor for update options
        public CustomerWindow(BlApi.IBL blAccessTemp, int CustomerId, CustomersWindow customersListTemp)
        {
            InitializeComponent();
            UpdateOptions.Visibility = Visibility.Visible; //The update options window will be shown
            localCustomersWindow = customersListTemp;
            blAccess = blAccessTemp;
            MyCustomer = blAccess.GetSingleCustomer(CustomerId);
            UpdateOptions.DataContext = MyCustomer;
            CustomerLocationTbx.Text = MyCustomer.Location.ToString();
            SentParcels.ItemsSource = MyCustomer.ParcelsFromCustomerList;
            RecievedParcels.ItemsSource = MyCustomer.ParcelsToCustomerList;
        }
        /// <summary>
        /// Button click event, which will enable the user to close the update window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            blAccess.UpdateCustomerData(MyCustomer.Id, CustomerNameTbx.Text, CustomerPhoneTbx.Text);
            MessageBox.Show("Your update was done successfully");
            localCustomersWindow.selectionOptions();
            this.Close();
        }
        private void SentParcelsTbx_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelsListWindow parcelsListWindow = new(blAccess);
            ParcelByCustomer temp = (ParcelByCustomer)SentParcels.SelectedItem;
            new ParcelWindow(blAccess, temp.Id, parcelsListWindow).Show();
        }

        private void RecievedParcelsTbx_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelsListWindow parcelsListWindow = new(blAccess);
            ParcelByCustomer temp = (ParcelByCustomer)RecievedParcels.SelectedItem;
            new ParcelWindow(blAccess, temp.Id, parcelsListWindow).Show();
        }
        private void CloseUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            localCustomersWindow.selectionOptions();
        }
        #endregion Update
    }
}
