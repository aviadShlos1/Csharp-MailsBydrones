//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Level 3
//Brief: Improve the presentation and add user interface
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
        /// A button click event which gets the user to the drones window display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DroneButton_Click(object sender, RoutedEventArgs e)
        {
            new DronesListWindow(blAccess).Show();
            this.Close();
        }
        /// <summary>
        /// A button click event which gets the user to the base stations window display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseButton_Click(object sender, RoutedEventArgs e)
        {
            new BaseStationListWindow(blAccess).Show();
            this.Close();
        }
        /// <summary>
        /// A button click event which gets the user to the customers window display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomersButton_Click(object sender, RoutedEventArgs e)
        {
            new CustomersListWindow(blAccess).Show();
            this.Close();
        }
        /// <summary>
        /// A button click event which gets the user to the parcels window display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParcelsButton_Click_1(object sender, RoutedEventArgs e)
        {
            new ParcelsListWindow(blAccess).Show();
            this.Close();
        }
        /// <summary>
        /// A button click event which closes this window and gets the user to the main login window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClosingBt_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
