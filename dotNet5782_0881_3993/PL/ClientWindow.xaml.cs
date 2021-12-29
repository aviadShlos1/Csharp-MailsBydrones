﻿using System;
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
        public ClientWindow(BlApi.IBL bl, int ClientId)
        {
            InitializeComponent();
            blAccess = bl;

            MyCustomer = blAccess.GetSingleCustomer(ClientId);
            DataContext = MyCustomer;
            listOfCustomerSend.ItemsSource = blAccess.GetSingleCustomer(MyCustomer.Id).ParcelsFromCustomerList;

            listOfCustomerReceive.ItemsSource = blAccess.GetSingleCustomer(MyCustomer.Id).ParcelsToCustomerList;

            WeightTbx.ItemsSource = Enum.GetValues(typeof(WeightCategoriesBL));
            PriorityTbx.ItemsSource = Enum.GetValues(typeof(PrioritiesBL));
            IEnumerable<int> customersId = blAccess.GetCustomersBl().Select(x => x.Id);
            TargetIdTbx.ItemsSource = customersId;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // adding the new parcel for sending
            AssignCustomerToParcel myAssignSenderToParcel = new() { CustId = MyCustomer.Id, CustName = MyCustomer.Name };
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
                listOfCustomerSend.ItemsSource = blAccess.GetSingleCustomer(MyCustomer.Id).ParcelsFromCustomerList;
            }
        }
    }
}
