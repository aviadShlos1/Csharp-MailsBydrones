//Names: Aviad Shlosberg       314960881      
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
        public MainWindow()
        {
            InitializeComponent();
            //media.Source = new Uri(Environment.CurrentDirectory + @"\load.gif");
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new ListsDisplayWindow().Show();
            this.Close();
        }
    }
}
