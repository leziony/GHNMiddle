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
    /// Interaction logic for WindowAdminLog.xaml
    /// </summary>
    public partial class WindowAdminLog : Window
    {
        MainWindow conn = new MainWindow();
        public WindowAdminLog()
        {
            InitializeComponent();
            MessageBox.Show("Funkcja w testach. \n Użyj nazwy i hasła test by zalogować się.", "Testy");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //var test = MessageBox.Show("Funkcja w testach. \n Czy chcesz sprawdzić czy to działa?","Testy",MessageBoxButton.YesNo,MessageBoxImage.Question);
            //komentarz do usuniecia
            /* if ( login.Text == "Test" && password.Text == "Test")
             {
                 WindowAdmin a = new WindowAdmin();
                 a.Show();
                 this.Close();
             }
             else
             {
                 MessageBox.Show("Nieprawidłowe dane");
                 Application.Current.MainWindow.Show();
                 this.Close();
             }
            */
            conn.connectsql("server=localhost;uid=root;pwd=admin;database=ghndata;");
            conn.conn.Open();
            string sql = "SELECT ID FROM users WHERE password=?pap";
            MySqlCommand cmd = new MySqlCommand(sql, conn.conn);
            cmd.Parameters.Add(new MySqlParameter("pap", password.Text));
            int controlID;
            MySqlDataReader reader = cmd.ExecuteReader();
            if(reader.HasRows)
            {
                reader.Read();
                controlID = int.Parse(reader["ID"].ToString());
                if(controlID == 1)
                {
                    WindowAdmin a = new WindowAdmin();
                    a.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Nieprawidłowe dane");
                    Application.Current.MainWindow.Show();
                    this.Close();

                }

            }
            else
            {
                MessageBox.Show("Nieprawidłowe dane");
                Application.Current.MainWindow.Show();
                this.Close();
            }


        }
    }
}
