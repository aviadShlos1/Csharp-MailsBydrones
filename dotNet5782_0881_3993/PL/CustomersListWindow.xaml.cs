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
using System.Collections.ObjectModel;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomersListWindow.xaml
    /// </summary>
    public partial class CustomersListWindow : Window
    {
        private BlApi.IBL blAccess;
        public ObservableCollection<CustomerToList> myCustomerPl;

        /// <summary>
        /// ctor for initialize the observable list
        /// </summary>
        /// <param name="blAccessTemp"> bl access </param>
        public CustomersListWindow(BlApi.IBL blAccessTemp)
        {
            InitializeComponent();
            blAccess = blAccessTemp;
            myCustomerPl = new ObservableCollection<CustomerToList>(blAccess.GetCustomersBl());
            CustomersListView.DataContext = myCustomerPl;
        }
        public void selectionOptions()
        {
            CustomersListView.ItemsSource = blAccess.GetCustomersBl();
        }
        /// <summary>
        /// A button click event, the add drone window will be opened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(blAccess, this).ShowDialog();
            CustomersListView.Items.Refresh();
        }
        /// <summary>
        /// A button click event, this window will be closed and the lists display will be shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClosingWindowButton_Click(object sender, RoutedEventArgs e)
        {
            new ListsDisplayWindow().Show();
            this.Close();
        }

        private void CustomersListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CustomerToList temp = (CustomerToList)CustomersListView.SelectedItem;
            new CustomerWindow(blAccess, temp.Id, this).Show();
        }

        
    }
}
