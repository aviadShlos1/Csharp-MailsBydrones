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
    /// Interaction logic for ParcelsListWindow.xaml
    /// </summary>
    public partial class ParcelsListWindow : Window
    {
        private BlApi.IBL blAccess;
        public ParcelsListWindow(BlApi.IBL blAccessTemp)
        {
            InitializeComponent();
            blAccess = blAccessTemp;
            ParcelsListView.ItemsSource = blAccess.GetParcelsBl();
        }

        private void AddParcelButton_Click(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(blAccess, this).Show();
        }

        private void ClosingButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ParcelsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
