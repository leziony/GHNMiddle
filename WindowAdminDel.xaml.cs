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
using System.Windows.Shapes;

namespace GHNMiddle
{
    /// <summary>
    /// Interaction logic for WindowAdminDel.xaml
    /// </summary>
    public partial class WindowAdminDel : Window
    {
        MainWindow connect = new MainWindow();
        string database;
        bool connectopen = false;
        public WindowAdminDel(string database)
        {
            InitializeComponent();
            this.database = database;
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            connect.connectsql("server=localhost;uid=root;pwd=admin;database=ghndata;");
            connect.conn.Open();
            connectopen = true;
            string sql;
            if (database == "users")
            {
                sql = "DELETE FROM users WHERE ID = ?del";
                MySqlCommand cm = new MySqlCommand(sql, connect.conn);
                cm.Parameters.Add(new MySqlParameter("del", int.Parse(deleteID.Text)));
                if (cm.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show("Failure");
                }
                connect.conn.Close();
                this.Close();
            }
            else if (database == "discounts")
            {
                sql = "DELETE FROM discounts WHERE discount_code = ?del";
                MySqlCommand cm = new MySqlCommand(sql, connect.conn);
                cm.Parameters.Add(new MySqlParameter("del", deleteID.Text));
                if (cm.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show("Failure");
                }
                connect.conn.Close();
                this.Close();
            }
            else if (database == "tarrif_code")
            {
                sql = "DELETE FROM tarrif_code WHERE tarrifID = ?del";
                MySqlCommand cm = new MySqlCommand(sql, connect.conn);
                cm.Parameters.Add(new MySqlParameter("del", deleteID.Text));
                if (cm.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Success");
                }
                else
                {
                    MessageBox.Show("Failure");
                }
                connect.conn.Close();
                this.Close();
            }
            else
            {
                MessageBox.Show("Error");
                this.Close();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(connectopen== true)
            {
                connect.conn.Dispose();
                connectopen = false;
            }
            connect.Close();

        }
    }
}
