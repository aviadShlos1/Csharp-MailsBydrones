﻿//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Targil3
//brief: In this program we built the presentation layer
using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for DronesListWindow.xaml
    /// </summary>
    public partial class DronesListWindow : Window
    {
        private BlApi.IBL blAccess;
        public ObservableCollection<DroneToList> myDronesPl;

        /// <summary>
        /// ctor for initialize the observable list
        /// </summary>
        /// <param name="blAccessTemp"> bl access </param>
        public DronesListWindow(BlApi.IBL blAccessTemp)
        {
            InitializeComponent();
            blAccess = blAccessTemp;
            myDronesPl = new ObservableCollection<DroneToList>(blAccess.GetDronesBl());
            myDronesPl.CollectionChanged += myDronesPl_CollectionChanged;
            DronesListView.ItemsSource = myDronesPl;
            //DronesListView.DataContext = myDronesPl;
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatusesBL));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategoriesBL));
        }

        /// <summary>
        /// function that update the observable for every change which is done to the observable list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myDronesPl_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
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
            new DroneWindow(blAccess,this).ShowDialog();
            DronesListView.Items.Refresh();
        }
        /// <summary>
        /// Bonus : Auxiliary method that taking into consideration all the selection options 
        /// </summary>
        public void selectionOptions()
        {
            if (WeightSelector.SelectedItem == null && StatusSelector.SelectedItem == null) //the user select none
            {
                DronesListView.ItemsSource = myDronesPl.ToList();
            }
            else if (WeightSelector.SelectedItem == null) // the user selected by status
            {
                DronesListView.ItemsSource = myDronesPl.Where(x => x.DroneStatus == (DroneStatusesBL)StatusSelector.SelectedItem);
            }
            else if (StatusSelector.SelectedItem == null)// the user selected by weight
            {
                DronesListView.ItemsSource = myDronesPl.Where(x => x.DroneWeight == (WeightCategoriesBL)WeightSelector.SelectedItem);
            }
            else // the user selected both by status and weight
            {
                DronesListView.ItemsSource = myDronesPl.Where(x => x.DroneStatus == (DroneStatusesBL)StatusSelector.SelectedItem && x.DroneWeight == (WeightCategoriesBL)WeightSelector.SelectedItem);
            }
        }
        /// <summary>
        /// A selection event, the user will choose between the status categories 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectionOptions();
        }
        /// <summary>
        /// A selection event, the user will choose between the weight categories 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectionOptions();
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
            selectionOptions();
            StatusRbtn.IsEnabled = true;
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
        /// <summary>
        /// A closing window. The lists display again
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClosingWindowButton_Click(object sender, RoutedEventArgs e)
        {
            new ListsDisplayWindow().Show();
            this.Close();
        }
        /// <summary>
        /// Grouping by the status and display the headers of them
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusRbtn_Click(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(DronesListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("DroneStatus");
            view.GroupDescriptions.Add(groupDescription);
            StatusRbtn.IsEnabled = false;
        }

        /// <summary>
        /// draging the window by holding it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Drag(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
