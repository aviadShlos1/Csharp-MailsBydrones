﻿//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil3
//brief: In this program we built the presentation layer
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
    /// Interaction logic for DronesListWindow.xaml
    /// </summary>
    public partial class DronesListWindow : Window
    {
        private BlApi.IBL blAccess;
        public DronesListWindow(BlApi.IBL blAccessTemp)
        {
            InitializeComponent();
            blAccess = blAccessTemp;
            DronesListView.ItemsSource = blAccess.GetDronesBl();
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatusesBL));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategoriesBL));
            
        }
        /// <summary>
        /// Bonus : Auxiliary method that taking into consideration all the selection options 
        /// </summary>
        public void selectionOptions()
        {
            if (WeightSelector.SelectedItem == null && StatusSelector.SelectedItem == null) //the user select none
            {
                DronesListView.ItemsSource = blAccess.GetDronesBl();
            }
            else if (WeightSelector.SelectedItem == null) // the user selected by status
            {
                DronesListView.ItemsSource = blAccess.GetDronesBl(x => x.DroneStatus == (DroneStatusesBL)StatusSelector.SelectedItem);
            }
            else if (StatusSelector.SelectedItem == null)// the user selected by weight
            {
                DronesListView.ItemsSource = blAccess.GetDronesBl(x => x.DroneWeight == (WeightCategoriesBL)WeightSelector.SelectedItem);
            }
            else // the user selected both by status and weight
            {
                DronesListView.ItemsSource = blAccess.GetDronesBl(x => x.DroneStatus == (DroneStatusesBL)StatusSelector.SelectedItem && x.DroneWeight == (WeightCategoriesBL)WeightSelector.SelectedItem);
            }
        }
       
        /// <summary>//
        /// A selection event, the user will choose between the status categories 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_StatusSelection(object sender, SelectionChangedEventArgs e)//
        {
            selectionOptions();
        }
      
        /// <summary>
        /// A selection event, the user will choose between the weight categories 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_WeightSelection(object sender, SelectionChangedEventArgs e)
        {
            selectionOptions();
        }

        /// <summary>
        /// A button click event, the add drone window will be opened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddDroneButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(blAccess,this).Show();
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
        /// <summary>
        /// A reset button click event, the selection will be cleared
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            StatusSelector.SelectedItem = null;
            WeightSelector.SelectedItem = null;
            //DroneListView.ItemsSource = droneToLists;
            selectionOptions();
        }
        /// <summary>
        /// A double click event. The user will click double click on the wanted drone, in that he will be able to do some action with it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DronesListView_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneToList temp = (DroneToList)DronesListView.SelectedItem;
            new DroneWindow(blAccess,temp.DroneId,this).Show();
        }
    }
}
