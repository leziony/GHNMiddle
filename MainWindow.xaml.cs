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
        MySqlConnector.MySqlConnection conn;
        public bool connectsql(string connection)
        {
            try
            {
                conn = new MySqlConnector.MySqlConnection(connection);
                conn.Open();
            }
            catch (MySqlException ex) { return false; }
            return true;
        }
        
        
        public MainWindow()
        {
            InitializeComponent();
            if (!connectsql("server=localhost;uid=root;pwd=admin;database=ghndata"))
            {
                MessageBox.Show("Failure");
            }
            else
            {
                string sql = "SELECT * FROM tarrif_code";
                MySqlCommand cmd = new MySqlCommand(sql,conn);
                MySqlDataReader dr = cmd.ExecuteReader();  
                while (dr.Read())
                {
                    MessageBox.Show(dr[0] + " " + dr[1].ToString());
                }

            }
        }

        private void ButtonGA_Click(object sender, RoutedEventArgs e)
        {
            WindowGA GA = new WindowGA(); 
            GA.Show();
            this.Hide();
        }

        private void ButtonCA_Click(object sender, RoutedEventArgs e)
        {
            //to be done soon
        }
    }
}
