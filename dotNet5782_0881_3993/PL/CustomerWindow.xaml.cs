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
                    CustomerId = int.Parse(IdTbx.Text),
                    CustomerName = NameTbx.Text,
                    CustomerPhone = PhoneTbx.Text,
                    CustomerLocation = new Location() { Latitude = double.Parse(LatitudeTbx.Text), Longitude = double.Parse(LongitudeTbx.Text) },

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
    }
}
