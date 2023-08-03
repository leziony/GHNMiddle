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
    /// Interaction logic for WindowMod.xaml
    /// </summary>
    public partial class WindowMod : Window
    {
        MainWindow con = new MainWindow();
        string id;
        public WindowMod(string id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            con.connectsql("server=localhost;uid=root;pwd=admin;database=ghndata;");
            con.conn.Open();
            string sql = "UPDATE " + id + " SET tarrifcode = ?t2 WHERE tarrifcode = ?t1";
            MySqlCommand cmd = new MySqlCommand(sql,con.conn);
            cmd.Parameters.Add(new MySqlParameter("t2",newTarrif.Text));
            cmd.Parameters.Add(new MySqlParameter("t1", oldTarrif.Text));
            cmd.ExecuteNonQuery();
            con.conn.Close();
            con.Close();
            this.Close();
        }
    }
}
