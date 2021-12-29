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
using System.Media;
using System.Windows.Media.Animation;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region main window
        /// <summary> the constractor start the intlize consractor of the data </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        // we crate an obejt that give us accses to the ibl intrface  
        public BlApi.IBL AccessIbl = BlApi.BlFactory.GetBl();

        /// <summary>
        /// Login to a manager or client interface.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Blogin_Click(object sender, RoutedEventArgs e)
        {

            switch (Blogin.Content)
            {
                case "Admin":
                    new ListsDisplayWindow().Show();
                    break;
                case "User":
                    try
                    {
                        AccessIbl.GetSingleCustomer(int.Parse(TBuserID.Password)); //לשים לב שזה מקפיץ חריגה אם הלקוח נכנס ללא שם משתמש
                        new ClientWindow(AccessIbl, int.Parse(TBuserID.Password)).Show();
                    }
                    catch (BO.NotExistException ex)
                    {
                        MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    break;
                default:
                    break;
            }
            this.Close();
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enter_Loaded(object sender, RoutedEventArgs e)
        {
            AddOn.Opacity = 0;
            DoubleAnimation Animmation = new DoubleAnimation(0, 100, TimeSpan.FromSeconds(5));
            PBloading.BeginAnimation(ProgressBar.ValueProperty, Animmation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PBloading_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (PBloading.Value == 100)
            {
                Blogin.IsEnabled = true;
                DoubleAnimation doubleAnimmation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(5));
                AddOn.BeginAnimation(Grid.OpacityProperty, doubleAnimmation);
                DoubleAnimation DSF = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(1));
                Disiaper.BeginAnimation(Grid.OpacityProperty, DSF);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BNewUser_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            new CustomerWindow(AccessIbl, new CustomersListWindow(AccessIbl)).Show();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TICadmin_GotFocus(object sender, RoutedEventArgs e)
        {
            Blogin.Visibility = Visibility.Visible;
            Blogin.Content = "Admin";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TIUser_GotFocus(object sender, RoutedEventArgs e)
        {
            Blogin.Visibility = Visibility.Visible;
            Blogin.Content = "User";
            
        }

        private void TBadmin_KeyUp(object sender, KeyEventArgs e)
        {
            if (TBadmin.Text.Length != 0)
            {
                Blogin.IsEnabled = true;
            }
            else
            {
                Blogin.IsEnabled = false;
            }
        }

        private void TBuserID_KeyUp(object sender, KeyEventArgs e)
        {
            if (TBuserID.Password.Length != 0)
            {
                Blogin.IsEnabled = true;
            }
            else
            {
                Blogin.IsEnabled = false;
            }
        }
    }
}
