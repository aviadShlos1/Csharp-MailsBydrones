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
using BO;
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
            StatusSelector.ItemsSource = Enum.GetValues(typeof(ParcelStatus));
            PrioritySelector.ItemsSource = Enum.GetValues(typeof(PrioritiesBL));
        }
        public void selectionOptions()
        {           
            if (PrioritySelector.SelectedItem == null && StatusSelector.SelectedItem == null) //the user select none
            {
                ParcelsListView.ItemsSource = blAccess.GetParcelsBl();
            }
            else if (PrioritySelector.SelectedItem == null) // the user selected by status
            {
                ParcelsListView.ItemsSource = blAccess.GetParcelsBl(x => x.ParcelStatus == (ParcelStatus)StatusSelector.SelectedItem);
            }
            else if (StatusSelector.SelectedItem == null)// the user selected by weight
            {
                ParcelsListView.ItemsSource = blAccess.GetParcelsBl(x => x.Priority == (PrioritiesBL)PrioritySelector.SelectedItem);
            }
            else // the user selected both by status and weight
            {
                ParcelsListView.ItemsSource = blAccess.GetParcelsBl(x => x.ParcelStatus == (ParcelStatus)StatusSelector.SelectedItem && x.Priority == (PrioritiesBL)PrioritySelector.SelectedItem);
            }
        }

        private void AddParcelButton_Click(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(blAccess, this).Show();
        }

        private void ClosingButton_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            StatusSelector.SelectedItem = null;
            PrioritySelector.SelectedItem = null;
            selectionOptions();
        }
        private void ParcelsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectionOptions();
        }

        private void PrioritySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectionOptions();
        }

    }
}
