﻿//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Level 3
//brief: Improve the presentation and add user interface
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
        private CustomersListWindow localCustomersWindow;
        #region Add
        /// <summary>
        /// Ctor for Add
        /// </summary>
        /// <param name="blAccessTemp"></param>
        /// <param name="customersListTemp">Given in order to Enable update the customers list</param>
        public CustomerWindow(BlApi.IBL blAccessTemp, CustomersListWindow customersListTemp)
        {
            InitializeComponent();
            AddOption.Visibility = Visibility.Visible; // the add option will be shown
            blAccess = blAccessTemp;
            localCustomersWindow = customersListTemp;
        }
        /// An add click buttun event. When the user will click this button, the customer will be added
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // adding the new MyCustomer details
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
                MessageBox.Show("This MyCustomer is already exists");
                IdTbx.BorderBrush = Brushes.Red;
                IdTbl.Background = Brushes.Red;
            }
        }
        /// <summary>
        ///  A click buttun event. When the user will click this button, the add options window will be closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseAddButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            localCustomersWindow.selectionOptions();
        }
        #endregion Add

        #region Update 
        public CustomerBL MyCustomer;
        /// <summary>
        /// Ctor for update options
        /// </summary>
        /// <param name="blAccessTemp"></param>
        /// <param name="CustomerId">Get customer id to get the customer</param>
        /// <param name="customersListTemp">Given in order to Enable update the customers list</param>
        public CustomerWindow(BlApi.IBL blAccessTemp, int CustomerId, CustomersListWindow customersListTemp)
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
        /// <summary>
        /// Double click event, which will opens the window of the parcel that he clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SentParcelsTbx_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelsListWindow parcelsListWindow = new(blAccess);
            ParcelByCustomer temp = (ParcelByCustomer)SentParcels.SelectedItem;
            new ParcelWindow(blAccess, temp.Id, parcelsListWindow).Show();
        }
        /// <summary>
        /// Double click event, which will opens the window of the parcel that he clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecievedParcelsTbx_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelsListWindow parcelsListWindow = new(blAccess);
            ParcelByCustomer temp = (ParcelByCustomer)RecievedParcels.SelectedItem;
            new ParcelWindow(blAccess, temp.Id, parcelsListWindow).Show();
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
        #endregion Update
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
