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

namespace GHNMiddle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonGA_Click(object sender, RoutedEventArgs e)
        {
            WindowGA GA = new WindowGA(); 
            GA.Show();
            this.Hide();
        }

        private void ButtonCA_Click(object sender, RoutedEventArgs e)
        {
            //to be done after GA
        }
    }
}
