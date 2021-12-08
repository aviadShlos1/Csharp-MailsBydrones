﻿using System;
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
        private void ComboBox_StatusSelection(object sender, SelectionChangedEventArgs e)//
        {
            selectionOptions();
        }
        private void ComboBox_WeightSelection(object sender, SelectionChangedEventArgs e)
        {
            selectionOptions();
        }

        private void AddDroneButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(blAccess,this).Show();
        }
        private void ClosingWindowButton_Click(object sender, RoutedEventArgs e)
        {          
            new MainWindow().Show();
            this.Close();
        }
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            StatusSelector.SelectedItem = null;
            WeightSelector.SelectedItem = null;
            //DroneListView.ItemsSource = droneToLists;
            selectionOptions();
        }
        private void DronesListView_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneToList temp = (DroneToList)DronesListView.SelectedItem;
            new DroneWindow(blAccess,temp.DroneId,this).Show();
        }
    }
}
