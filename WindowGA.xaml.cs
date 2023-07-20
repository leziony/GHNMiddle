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

namespace GHNMiddle
{
    /// <summary>
    /// Logika interakcji dla klasy WindowGA.xaml
    /// </summary>
    public partial class WindowGA : Window
    {
        public WindowGA()
        {
            InitializeComponent();
        }

        private void WindowGA_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.MainWindow.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
