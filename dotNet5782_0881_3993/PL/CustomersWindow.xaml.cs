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
    /// Interaction logic for CustomersWindow.xaml
    /// </summary>
    public partial class CustomersWindow : Window
    {
        private BlApi.IBL blAccess;
        public CustomersWindow(BlApi.IBL blAccessTemp)
        {
            InitializeComponent();
            blAccess = blAccessTemp;
            CustomersListView.ItemsSource = blAccess.GetCustomersBl();
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
            new CustomerWindow(blAccess, this).Show();
        }
        /// <summary>
        /// A button click event, the add drone window will be opened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClosingWindowButton_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

        private void CustomersListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CustomerToList temp = (CustomerToList)CustomersListView.SelectedItem;
            new CustomerWindow(blAccess, temp.Id, this).Show();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
