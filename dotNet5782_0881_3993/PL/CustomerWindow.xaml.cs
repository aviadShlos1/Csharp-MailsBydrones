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
                CustomerBL newCustomer = new CustomerBL
                {
                    Id = int.Parse(IdTbx.Text),
                    Name = NameTbx.Text,
                    Phone = PhoneTbx.Text,
                    Location = new Location() { Latitude = double.Parse(LatitudeTbx.Text), Longitude = double.Parse(LongitudeTbx.Text) },

                };
                blAccess.AddCustomer(newCustomer);

                MessageBoxResult result = MessageBox.Show("The operation was done successfully");
                if (result == MessageBoxResult.OK)
                {
                    this.Close();
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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
            ParcelsFromCustomerListTbx.ItemsSource = MyCustomer.ParcelsFromCustomerList;
            ParcelsToCustomerListTbx.ItemsSource = MyCustomer.ParcelsToCustomerList;
        }
        /// <summary>
        /// Button click event, which will enable the user to close the update window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            localCustomersWindow.selectionOptions();
        }
        private void UpdateCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            blAccess.UpdateCustomerData(MyCustomer.Id, CustomerNameTbx.Text, CustomerPhoneTbx.Text);
            MessageBox.Show("Your update was done successfully");
            localCustomersWindow.selectionOptions();
            this.Close();
        }

        private void DeleteCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            int myIndex = blAccess.GetCustomersBl().FindIndex(x => x.Id == MyCustomer.Id);
            blAccess.GetCustomersBl().RemoveAt(myIndex);
            MessageBox.Show("Your delete was done successfully");
            this.Close();           
            localCustomersWindow.selectionOptions();
        }
        #endregion Update

        private void ParcelsFromCustomerListTbx_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelsListWindow parcelsListWindow = new(blAccess);
            ParcelByCustomer temp = (ParcelByCustomer)ParcelsFromCustomerListTbx.SelectedItem;
            new ParcelWindow(blAccess, temp.Id, parcelsListWindow).Show();
        }

        private void ParcelsToCustomerListTbx_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelsListWindow parcelsListWindow = new(blAccess);
            ParcelByCustomer temp = (ParcelByCustomer)ParcelsToCustomerListTbx.SelectedItem;
            new ParcelWindow(blAccess, temp.Id, parcelsListWindow).Show();
        }
    }
}
