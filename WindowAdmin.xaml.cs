using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for WindowAdmin.xaml
    /// </summary>
    public partial class WindowAdmin : Window
    {
        MainWindow conn = new MainWindow();
        string currentBase;
        public void DatabasePull()
        {
            conn.connectsql("server=localhost;uid=root;pwd=admin;database=ghndata;");
            conn.conn.Open();
            string sql = "SHOW TABLES IN ghndata";
            MySqlCommand cmd = new MySqlCommand(sql,conn.conn);
            MySqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                selectDatabase.Items.Add(dr.GetString(0));
            }
            conn.conn.Close();
            
        }
        public void TableInit()
        {
            conn.conn.Open();
            string sql = "SELECT * FROM " + currentBase;
            MySqlCommand cmd = new MySqlCommand(sql, conn.conn);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            conn.conn.Close();
            grinder.DataContext = dt;
        }
        public WindowAdmin()
        {
            InitializeComponent();
            DatabasePull();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentBase = selectDatabase.SelectedItem.ToString();
            TableInit();

        }

        private void WindowAdmin_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            conn.conn.Dispose();
            conn.Close();
            Application.Current.MainWindow.Show();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
