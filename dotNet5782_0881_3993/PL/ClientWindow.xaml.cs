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
    /// Interaction logic for ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        
        public BlApi.IBL blAccess;

        //object of ListView window.
        public ListView ListWindow;

        /// <summary> a bool to help us disable the x bootum  </summary>
        public bool ClosingWindow { get; private set; } = true;

        public CustomerBL Customer;

        public int indexSelected;
        public ClientWindow(BlApi.IBL bl, int ClientId)
        {
            InitializeComponent();
            blAccess = bl;

            Customer = blAccess.GetSingleCustomer(ClientId);
            //DataContext = Customer;
            listOfCustomerSend.ItemsSource = blAccess.GetSingleCustomer(Customer.Id).ParcelsFromCustomerList;
            listOfCustomerReceive.ItemsSource = blAccess.GetSingleCustomer(Customer.Id).ParcelsToCustomerList;
        }
    }
}
