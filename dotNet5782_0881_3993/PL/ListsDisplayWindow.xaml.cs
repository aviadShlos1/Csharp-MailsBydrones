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

namespace PL
{
    /// <summary>
    /// Interaction logic for ListsDisplayWindow.xaml
    /// </summary>
    public partial class ListsDisplayWindow : Window
    {
        private BlApi.IBL blAccess = BlApi.BlFactory.GetBl();

        public ListsDisplayWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// A button click event which lead the user to the drones window display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
      

        private void DroneButton_Click(object sender, RoutedEventArgs e)
        {
            new DronesListWindow(blAccess).Show();
            this.Close();
        }

        private void BaseButton_Click(object sender, RoutedEventArgs e)
        {
            new BaseStationListWindow(blAccess).Show();
            this.Close();
        }

        private void CustomersButton_Click(object sender, RoutedEventArgs e)
        {
            new CustomersWindow(blAccess).Show();
            this.Close();
        }

        private void ParcelsButton_Click_1(object sender, RoutedEventArgs e)
        {
            new ParcelsListWindow(blAccess).Show();
            this.Close();
        }
    }
}
