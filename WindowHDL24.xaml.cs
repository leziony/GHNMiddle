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
    /// Interaction logic for WindowHDL24.xaml
    /// </summary>
    public partial class WindowHDL24 : Window
    {
        string id;
        MainWindow connect = new MainWindow();
        public WindowHDL24(string id)
        {
            InitializeComponent();
            this.id = id;
            MessageBox.Show("Wykryto opłate HDL24, która jest opłatą za usługi specjalne. \n Proszę podać sumę HDL24 ze GHN i wprowadzić ją na następnym ekranie", "Opłata specjalna", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            connect.connectsql("server=localhost;uid=root;pwd=admin;database=ghndata;");
            connect.conn.Open();
            decimal costi = decimal.Parse(cost.Text.ToString());
            string sql = "UPDATE " + id + "temp SET cost = ?cost, ammount = 1 WHERE tarrifcode = 'HDL24'";
            MySqlCommand cmd = new MySqlCommand(sql, connect.conn);
            cmd.Parameters.Add(new MySqlParameter("cost", costi));
            cmd.ExecuteNonQuery();
            connect.conn.Dispose();
            connect.Close();
            this.Close();

        }
    }
}
