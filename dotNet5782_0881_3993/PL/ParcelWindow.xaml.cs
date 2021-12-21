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
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        private BlApi.IBL blAccess;
        private ParcelsListWindow localParcelsListWindow;
        public ParcelWindow(BlApi.IBL blAccessTemp, ParcelsListWindow parcelsListTemp)
        {
            InitializeComponent();
            AddOption.Visibility = Visibility.Visible; // the add option will be shown
            blAccess = blAccessTemp;
            localParcelsListWindow = parcelsListTemp;
            WeightTbx.ItemsSource = Enum.GetValues(typeof(WeightCategoriesBL));
            PriorityTbx.ItemsSource = Enum.GetValues(typeof(PrioritiesBL));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // adding the new parcel details
                AssignCustomerToParcel myAssignSenderToParcel = new() { Id = int.Parse(SenderIdTbx.Text) };
                AssignCustomerToParcel myAssignRecieverToParcel = new() { Id = int.Parse(TargetIdTbx.Text) };
                ParcelBl newParcel = new ParcelBl
                {
                    Sender = myAssignSenderToParcel,
                    Reciever = myAssignRecieverToParcel,
                    ParcelWeight = (WeightCategoriesBL)WeightTbx.SelectedItem,
                    Priority = (PrioritiesBL)PriorityTbx.SelectedItem
                };
                blAccess.AddParcel(newParcel);

                MessageBoxResult result = MessageBox.Show("The operation was done successfully");
                if (result == MessageBoxResult.OK)
                {
                    this.Close();
                    localParcelsListWindow.selectionOptions();
                }
            }
            catch (AlreadyExistException)
            {
                MessageBox.Show("This parcel is already exists");
                SenderIdTbx.BorderBrush = Brushes.Red;
                PriorityTbx.Background = Brushes.Red;
            }
        }

        private void PriorityTbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void WeightTbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
