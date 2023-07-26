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
            string sql = "CREATE TABLE IF NOT EXISTS "+ UserID.Text +"(" +
                "tarrifcode varchar(255)," +
                "ammount int," +
                "unit varchar(255)," +
                "cost decimal(15,2)" +
                ");";
            MySqlCommand cmd = new MySqlCommand(sql, wincon.conn);
            cmd.ExecuteNonQuery();
            wincon.changeId(UserID.Text);
            WindowGA Test = new WindowGA(UserID.Text);
            Test.Show();
            complete = true;
            wincon.conn.Dispose();
            wincon.Close();
            this.Close();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            wincon.Close();
            if(complete == false) { Application.Current.MainWindow.Show(); }
        }
    }
}
