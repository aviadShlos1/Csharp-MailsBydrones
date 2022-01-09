//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Level 3
//brief: Improve the presentation and add user interfaceusing System
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

        public CustomerBL MyCustomer;

        public int indexSelected;
        /// <summary>
        /// Ctor for Client window 
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="ClientId">Get client id to get the customer in this id</param>
        public ClientWindow(BlApi.IBL bl, int ClientId)
        {
            InitializeComponent();
            blAccess = bl;

            MyCustomer = blAccess.GetSingleCustomer(ClientId);
            DataContext = MyCustomer;
            //Display the parcels list, the first for parcels that he sent and the second for parcels that he recieved 
            OutgoingList.ItemsSource = blAccess.GetSingleCustomer(MyCustomer.Id).ParcelsFromCustomerList;
            IncomingList.ItemsSource = blAccess.GetSingleCustomer(MyCustomer.Id).ParcelsToCustomerList;
            //Needs for add parcel option 
            WeightTbx.ItemsSource = Enum.GetValues(typeof(WeightCategoriesBL));
            PriorityTbx.ItemsSource = Enum.GetValues(typeof(PrioritiesBL));
            IEnumerable<int> customersId = blAccess.GetCustomersBl().Select(x => x.Id);
            TargetIdTbx.ItemsSource = customersId;
        }
        /// <summary>
        /// Click event, when he click "Add" the details of the parcel will be add to the list and will display in the sends list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // adding the new parcel for sending
            AssignCustomerToParcel myAssignSenderToParcel = new() { CustId = MyCustomer.Id, CustName = MyCustomer.Name };
            AssignCustomerToParcel myAssignRecieverToParcel = new() { CustId = (int)TargetIdTbx.SelectedItem, CustName=blAccess.GetSingleCustomer((int)TargetIdTbx.SelectedItem).Name};
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
                OutgoingList.ItemsSource = blAccess.GetSingleCustomer(MyCustomer.Id).ParcelsFromCustomerList;
                OutgoingList.Items.Refresh();
            }

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(blAccess, MyCustomer.Id ,new CustomersListWindow(blAccess)).Show();
        }
    }
}
