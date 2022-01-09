//Names: Aviad Shlosberg       314960881      
//       Evyatar Levi Ben Ston 318753993 
//Level 3
//Brief: Improve the presentation and add user interface
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
using BlApi;
using MaterialDesignThemes.Wpf;
using System.Windows.Media.Animation;
using PL;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region main window
        /// <summary> the constructor starts the data initializing </summary>
        // An access objet that gives us accses to bl interface  
        internal readonly IBL AccessIbl = BlFactory.GetBl();
        public MainWindow()
        {
            InitializeComponent();
            //SoundPlayer player = new SoundPlayer(@"C:\Users\aviad\source\repos\dotNet5782_0881_3993\dotNet5782_0881_3993\PL\icons");
            //player.Load();
            //player.Play();
        }

        /// <summary>
        /// Login event that lead to the manager or client interface.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Blogin_Click(object sender, RoutedEventArgs e)
        {

            switch (LoginButton.Content)
            {
                case "Admin":
                    if (UserNameTextBox.Text == "Admin" && PasswordBox.Password == "00000")
                    {
                        new ListsDisplayWindow().Show();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("ERROR: Invalid username or password");
                        new MainWindow().Show();

                    }

                    break;
                case "User":
                    try
                    {
                        AccessIbl.GetSingleCustomer(int.Parse(TBuserID.Text)); 
                        new ClientWindow(AccessIbl, int.Parse(TBuserID.Text)).Show();
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
        /// Loading event - event which begins before the main window display  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enter_Loaded(object sender, RoutedEventArgs e)
        {
            AddOn.Opacity = 0;
            DoubleAnimation Animmation = new DoubleAnimation(0, 100, TimeSpan.FromSeconds(2));
            PBloading.BeginAnimation(ProgressBar.ValueProperty, Animmation);
        }

        /// <summary>
        /// Event for loading progress bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PBloading_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (PBloading.Value == 100)
            {
                LoginButton.IsEnabled = true;
                DoubleAnimation doubleAnimmation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(2));
                AddOn.BeginAnimation(Grid.OpacityProperty, doubleAnimmation);
                DoubleAnimation DSF = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(1));
                Disappear.BeginAnimation(Grid.OpacityProperty, DSF);
            }
        }

        /// <summary>
        /// click event - lead to the register interface
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BNewUser_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(AccessIbl, new CustomersListWindow(AccessIbl)).Show();
        }
        /// <summary>
        /// mouse focus event that open the admin enter tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TICadmin_GotFocus(object sender, RoutedEventArgs e)
        {
            LoginButton.Visibility = Visibility.Visible;
            LoginButton.Content = "Admin";
        }

        /// <summary>
        /// mouse focus event that open the user enter tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TIUser_GotFocus(object sender, RoutedEventArgs e)
        {
            LoginButton.Visibility = Visibility.Visible;
            LoginButton.Content = "User";
            
        }

        /// <summary>
        /// event for getting input in the text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void TBadmin_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (TBadmin.Text.Length != 0)
        //    {
        //        Blogin.IsEnabled = true;
        //    }
        //    else
        //    {
        //        Blogin.IsEnabled = false;
        //    }
        //}
        /// <summary>
        /// event for getting input in the password box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TBuserID_KeyUp(object sender, KeyEventArgs e)
        {
            if (TBuserID.Text.Length != 0)
            {
                LoginButton.IsEnabled = true;
            }
            else
            {
                LoginButton.IsEnabled = false;
            }
        }

       
    }
}
