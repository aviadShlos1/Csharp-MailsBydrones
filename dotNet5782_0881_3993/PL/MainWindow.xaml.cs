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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // access point between the bl to the pl
        private BlApi.IBL blAccess = BlApi.BlFactory.GetBl();
        //ObservableCollection<par> myDronesPl;
        //ObservableCollection<DroneToList> myDronesPl;
        //ObservableCollection<DroneToList> myDronesPl;

        public MainWindow()
        {            
            InitializeComponent();
        }
        /// <summary>
        /// A button click event which lead the user to the drones window display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowDronesButton_Click(object sender, RoutedEventArgs e)
        {
            new DronesListWindow(blAccess).Show();
            this.Close();
        }
        private void ShowBaseStationButton_Click(object sender, RoutedEventArgs e)
        {
            new BaseStationListWindow(blAccess).Show();
            this.Close();
        }

        private void ShowCustomersButton_Click(object sender, RoutedEventArgs e)
        {
            new CustomersWindow(blAccess).Show();
            this.Close();
        }

        private void ShowParcelsButton_Click(object sender, RoutedEventArgs e)
        {
            new ParcelsListWindow(blAccess).Show();
            this.Close();
        }
    }
}
