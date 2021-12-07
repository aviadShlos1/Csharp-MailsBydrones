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
        private DronesListWindow localDronesListWindow;
        private int[] BaseStationNum = new int[] { 0,1 };
        private int firstChargeStation = default;
        public DroneWindow(IBL.IBL blAccessTemp, DronesListWindow dronesListTemp)//C-tor for add option
        {
            InitializeComponent();
            blAccess = blAccessTemp;
            localDronesListWindow = dronesListTemp;
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategoriesBL));
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatusesBL));
            BaseStationIdSelector.ItemsSource = BaseStationNum;
        }
        public DroneWindow(IBL.IBL blAccessTemp)//C-tor for update options
        {
            InitializeComponent();
            blAccess = blAccessTemp;

        }
        private void WeightComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WeightSelector.SelectedItem = Enum.GetValues(typeof(WeightCategoriesBL));
        }
        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatusSelector.SelectedItem = Enum.GetValues(typeof(DroneStatusesBL));
        }
        private void BaseStationIdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BaseStationIdSelector.SelectedItem = BaseStationNum;
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DroneToList newDrone = new DroneToList
                {
                    DroneId = int.Parse(IdTbx.Text),
                    Model = ModelTbx.Text,
                    DroneWeight = (WeightCategoriesBL)WeightSelector.SelectedItem, 
                };
                firstChargeStation = (int)BaseStationIdSelector.SelectedItem;
                blAccess.AddDrone(newDrone,firstChargeStation);               
                MessageBoxResult result = MessageBox.Show("The operation was done successfully");
                if (result==MessageBoxResult.OK)
                {
                    this.Close();
                    localDronesListWindow.selectionOptions();
                }
            }
            catch (AlreadyExistException)
            {
                MessageBox.Show("This drone is already exists");
                IdTbx.BorderBrush = Brushes.Red;
                IdTbl.Background= Brushes.Red;
            }
        }

       
    }
}
