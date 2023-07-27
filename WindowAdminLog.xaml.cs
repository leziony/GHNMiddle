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
    /// Interaction logic for WindowAdminLog.xaml
    /// </summary>
    public partial class WindowAdminLog : Window
    {
        public WindowAdminLog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var test = MessageBox.Show("Funkcja w testach. \n Czy chcesz sprawdzić czy to działa?","Testy",MessageBoxButton.YesNo,MessageBoxImage.Question);
            if (test == MessageBoxResult.Yes)
            {
                MessageBox.Show(login.Text + " " + password.Text,"Testy");
            }
           
            this.Close();
        }
    }
}
