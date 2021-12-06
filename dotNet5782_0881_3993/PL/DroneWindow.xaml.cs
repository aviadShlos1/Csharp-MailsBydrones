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
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private IBL.IBL blAccess;
        public DroneWindow(IBL.IBL blAccessTemp)
        {
            InitializeComponent();
            blAccess = blAccessTemp;
            DroneToList newDrone = new({ DroneId=int.Parse(TextBlock.TextProperty)})
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategoriesBL));
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatusesBL));
        }

        private void WeightComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WeightSelector.SelectedItem = Enum.GetValues(typeof(WeightCategoriesBL));
        }
        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatusSelector.SelectedItem = Enum.GetValues(typeof(DroneStatusesBL));
        }

        private void Battery_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void Id_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            blAccess.AddDrone();
        }
    }
}
