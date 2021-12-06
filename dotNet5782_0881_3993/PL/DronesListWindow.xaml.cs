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
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for DronesListWindow.xaml
    /// </summary>
    public partial class DronesListWindow : Window
    {
        private IBL.IBL blAccess;
        public DronesListWindow(IBL.IBL blAccessTemp)
        {
            InitializeComponent();
            blAccess = blAccessTemp;
            DronesListView.ItemsSource = blAccess.GetDronesBl();
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatusesBL));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategoriesBL));
        }
        private void ComboBox_StatusSelection(object sender, SelectionChangedEventArgs e)
        {
            DronesListView.ItemsSource = blAccess.GetDronesBl(x => x.DroneStatus == (DroneStatusesBL)StatusSelector.SelectedItem);
        }
        private void ComboBox_WeightSelection(object sender, SelectionChangedEventArgs e)
        {
            DronesListView.ItemsSource = blAccess.GetDronesBl(x => x.DroneWeight == (WeightCategoriesBL)WeightSelector.SelectedItem);
        }
    }
}
