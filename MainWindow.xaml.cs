using MySqlConnector;
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
        public MySqlConnector.MySqlConnection conn;
        public string id;
        public bool connectsql(string connection)
        {
            try
            {
                conn = new MySqlConnector.MySqlConnection(connection);
                conn.Open();
            }
            catch (MySqlException ex) { return false; }
            conn.Close();
            return true;
        }

        public void changeId(string idr)
        {
            id=idr;
        }
        
        
        public MainWindow()
        {
            InitializeComponent();
        }
        public MainWindow(string s)
        {
            InitializeComponent();
            id = s;
        }

        private void ButtonGA_Click(object sender, RoutedEventArgs e)
        {
            WindowID ID = new WindowID(); 
            ID.Show();
            this.Hide();
        }

        private void ButtonCA_Click(object sender, RoutedEventArgs e)
        {
            //Placeholder for later
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Potentially to do
        }
    }
}
