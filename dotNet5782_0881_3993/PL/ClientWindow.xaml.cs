﻿//Names: Aviad Shlosberg       314960881      
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
            
            //Connecting the the combobox to parcels who sent by the client, and show the parcel ID.
            PickUpCbx.ItemsSource = blAccess.GetParcelsBl(x => x.ParcelStatus == ParcelStatus.Assigned &&
                      blAccess.GetSingleCustomer(MyCustomer.Id).ParcelsFromCustomerList.ToList().Exists(item => item.Id == x.Id));
            PickUpCbx.DisplayMemberPath = "Id";

            //Connecting the the combobox to parcels that the client should receive, and show the parcel ID.
            CBDeliverdList.ItemsSource = blAccess.GetParcelsBl(x => x.ParcelStatus == ParcelStatus.PickedUp &&
                      blAccess.GetSingleCustomer(MyCustomer.Id).ParcelsToCustomerList.ToList().Exists(item => item.Id == x.Id));
            CBDeliverdList.DisplayMemberPath = "Id";
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


        #region PickUp combobox

        /// <summary>
        /// The function handles in case a package is selected for collection confirmation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBSendToPickUp_Checked(object sender, RoutedEventArgs e)
        {
            if (CBPickUpList.SelectedItem != null)//check if a package has been selected in combobox.
            {
                //int id = ((ParcelToList)CBPickUpList.SelectedItem).Id;
                PickedUp(blAccess.GetSingleParcel(((ParcelToList)CBPickUpList.SelectedItem).Id).MyDrone.Id);
            }
            else
            {
                MessageBox.Show("ERROR, unchecked parcel to pick up", MessageBoxButton.OK, MessageBoxImage.Error);
                CBSendToPickUp.IsChecked = false; //update the CheckBox to uncheck.
            }
        }

        /// <summary>
        /// picked up the parcel and updates the views accordingly.
        /// </summary>
        /// <param name="DroneId">drone Id</param>
        private void PickedUp(int DroneId)
        {
            try
            {
                //IsEnabled = false;
                blAccess.PickUpParcel(DroneId); //Activation of the PickedUp function in the BL layer.
                MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        //IsEnabled = true;
                        CBSendToPickUp.IsChecked = false; //update the CheckBox to uncheck.

                        //Update the combobox of parcels who sent by the client and have not yet been pickup.
                        CBPickUpList.ItemsSource = blAccess.GetParcelsBl(x => x.ParcelStatus == ParcelStatus.Assigned &&
                                blAccess.GetSingleCustomer(MyCustomer.Id).ParcelsFromCustomerList.ToList().Exists(item => item.Id == x.Id));
                        CBPickUpList.DisplayMemberPath = "Id";

                        //Update the Biding of client details.
                        MyCustomer = blAccess.GetSingleCustomer(MyCustomer.Id);
                        DataContext = MyCustomer;

                        //Update the list of parcels from the customer.
                        OutgoingList.ItemsSource = blAccess.GetSingleCustomer(MyCustomer.Id).ParcelsFromCustomerList;
                        break;

                    default:
                        break;
                }
            }
            catch (NotExistException ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                CBSendToPickUp.IsChecked = false; //update the CheckBox to uncheck.
            }
            catch (CannotPickUpException ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                CBSendToPickUp.IsChecked = false; //update the CheckBox to uncheck.
            }
        }

        /// <summary>
        /// Reset the ComboBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BRrestComboBox_Click(object sender, RoutedEventArgs e)
        {
            CBPickUpList.SelectedItem = null;
        }
        #endregion PickUp combobox

        #region Deliverd combobox

        /// <summary>
        /// The function handles in case a package is selected for deliverd confirmation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBSendToDeliverd_Checked(object sender, RoutedEventArgs e)
        {
            if (CBDeliverdList.SelectedItem != null)//check if a package has been selected in combobox.
            {
                //int id = ((ParcelToList)CBDeliverdList.SelectedItem).Id;
                DeliveryPackage(blAccess.GetParcel(((ParcelToList)CBDeliverdList.SelectedItem).Id).MyDrone.Id);
            }
            else
            {
                MessageBox.Show(" לא נבחרה חבילה לאספקה", "!שגיאה", MessageBoxButton.OK, MessageBoxImage.Error);
                CBSendToDeliverd.IsChecked = false;//update the CheckBox to uncheck.
            }
        }

        /// <summary>
        /// deliverd the parcel and updates the views accordingly.
        /// </summary>
        /// <param name="DroneId"></param>
        private void DeliveryPackage(int DroneId)
        {
            try
            {
                //IsEnabled = false;
                blAccess.SupplyParcel(DroneId);//Activation of the Delivery function in the BL layer.
                MessageBoxResult result = MessageBox.Show("The operation was successful", "info", MessageBoxButton.OK, MessageBoxImage.Information);
                switch (result)
                {
                    case MessageBoxResult.OK:
                        //IsEnabled = true;
                        CBSendToDeliverd.IsChecked = false;//update the CheckBox to uncheck.

                        //Update the combobox of parcels that the client should receive and have not yet been Delivered.
                        SupplyList_comboBox.ItemsSource = blAccess.GetParcelsBl(x => x.ParcelStatus == ParcelStatus.PickedUp &&
                                 blAccess.GetSingleCustomer(MyCustomer.Id).ParcelsToCustomerList.ToList().Exists(item => item.Id == x.Id));
                        SupplyList_comboBox.DisplayMemberPath = "Id";

                        //Update the Biding of client details.
                        MyCustomer = blAccess.GetSingleCustomer(MyCustomer.Id);
                        DataContext = MyCustomer;

                        //Update the list of parcels to the customer.
                        IncomingList.ItemsSource = blAccess.GetSingleCustomer(MyCustomer.Id).ParcelsToCustomerList;
                        break;

                    default:
                        break;
                }
            }
            catch (DeliveryCannotBeMade ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                //IsEnabled = true;
                CBSendToDeliverd.IsChecked = false;
            }
        }

        /// <summary>
        /// Reset the ComboBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BRrestComboBox2_Click(object sender, RoutedEventArgs e)
        {
            CBDeliverdList.SelectedItem = null;
        }
        #endregion Deliverd combobox

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
