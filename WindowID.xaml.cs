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
    /// Logika interakcji dla klasy WindowID.xaml
    /// </summary>
    public partial class WindowID : Window
    {
        MainWindow wincon = new MainWindow();
        bool complete = false;
        public WindowID()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            wincon.connectsql("server=localhost;uid=root;pwd=admin;database=ghndata;");
            wincon.conn.Open();
            string sql;
            sql = "SELECT * FROM users WHERE nazwisko = ?t1 AND password = ?t2 LIMIT 1";
            MySqlCommand cmd = new MySqlCommand(sql, wincon.conn);
            cmd.Parameters.Add(new MySqlParameter("t1", UserID.Text));
            cmd.Parameters.Add(new MySqlParameter("t2",password.Text));
            MySqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                rdr.Read();
                string pass = rdr["password"].ToString();
                if (pass == password.Text)
                {
                    int id = (int)rdr["ID"];
                    wincon.conn.Close();
                    wincon.conn.Open();
                    sql = "CREATE TABLE IF NOT EXISTS " + id.ToString() + "temp (" +
                    "tarrifcode varchar(255)," +
                    "ammount int," +
                    "unit varchar(255)," +
                    "cost decimal(15,2)" +
                    ");";
                    cmd = new MySqlCommand(sql, wincon.conn);
                    cmd.ExecuteNonQuery();
                    wincon.changeId(UserID.Text);
                    WindowGA Test = new WindowGA(UserID.Text);
                    Test.Show();
                    complete = true;
                    wincon.conn.Dispose();
                    wincon.Close();
                    this.Close();

                }
                else
                {
                    MessageBox.Show("Error at login!","Error");
                    wincon.conn.Close();
                }
            }
            else
            {
                MessageBox.Show("Error at login!", "Error");
                wincon.conn.Close();
            }
          

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            wincon.conn.Dispose();
            wincon.Close();
            if(complete == false) { Application.Current.MainWindow.Show(); }
        }
    }
}
