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
    /// Interaction logic for WindowAdminAdd.xaml
    /// </summary>
    public partial class WindowAdminAddMod : Window
    {
        string modifyID ="";
        bool isDiscount;
        bool isUser;
        MainWindow connect = new MainWindow();
        public void modifyValues()
        {
            if(modifyID == "")
            {
                MessageBox.Show("Problem z importem wiadomości. \n Jeżeli ta wartość nie istnieje, prosze nacisnąć przycisk Add.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }
            else
            {
                connect.connectsql("server=localhost;uid=root;pwd=admin;database=ghndata;");
                string sql;
                connect.conn.Open();
                if (isDiscount == true)
                {
                    sql = "SELECT * FROM discounts WHERE discount_code = ?value LIMIT 1";
                    MySqlCommand cmd = new MySqlCommand(sql,connect.conn);
                    cmd.Parameters.Add(new MySqlParameter("value", modifyID));
                    discountTab.Focus();
                    MySqlDataReader rd = cmd.ExecuteReader();  
                    rd.Read();
                    discountName.Text = rd["discount_code"].ToString();
                    discountValue.Text = rd["discount_value"].ToString();
                    if (rd["is_percent"].ToString()=="1")
                    {
                        discountPercent.IsChecked = true;
                    }
                    else
                    {
                        discountPercent.IsChecked = false;
                    }    


                }
                else if (isUser == true)
                {
                    sql = "SELECT * FROM users WHERE ID = ?value";
                    MySqlCommand cmd = new MySqlCommand(sql, connect.conn);
                    cmd.Parameters.Add(new MySqlParameter("value", modifyID));
                    usersTab.Focus();
                    MySqlDataReader rd = cmd.ExecuteReader();
                    rd.Read();
                    userID.Text = rd["ID"].ToString();
                    userFirst.Text = rd["imie"].ToString();
                    userLast.Text = rd["nazwisko"].ToString();
                    userPass.Text = rd["Password"].ToString();
                }
                else
                {
                    sql = "SELECT * FROM tarrif_code WHERE tarrifID = ?value";
                    MySqlCommand cmd = new MySqlCommand(sql, connect.conn);
                    cmd.Parameters.Add(new MySqlParameter("value", modifyID));
                    tarrifTab.Focus();
                    MySqlDataReader rd = cmd.ExecuteReader();
                    rd.Read();
                    tarrifCode.Text = rd["tarrifID"].ToString();
                    tarrifCost.Text = rd["cost"].ToString();
                }
                connect.conn.Close();

            }
        }
        public WindowAdminAddMod()
        {
            InitializeComponent();
            connect.connectsql("server=localhost;uid=root;pwd=admin;database=ghndata;");
        }
        public WindowAdminAddMod(string modifyID, bool isDiscount , bool isUser)
        {
            this.modifyID = modifyID;
            this.isDiscount = isDiscount;
            this.isUser = isUser;
            InitializeComponent();
            modifyValues();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            connect.conn.Dispose();
            connect.Close();
        }

        private void tarrifButton_Click(object sender, RoutedEventArgs e)
        {
            decimal cena = decimal.Parse(tarrifCost.Text);
            if (modifyID != "")
            {
                string sql = "UPDATE tarrif_code SET tarrifID = ?t1, cost = ?t2 WHERE tarrifID=?asd";
                connect.conn.Open();
                MySqlCommand cmd = new MySqlCommand(sql, connect.conn);
                cmd.Parameters.Add(new MySqlParameter("t1", tarrifCode.Text));
                cmd.Parameters.Add(new MySqlParameter("t2", cena));
                cmd.Parameters.Add(new MySqlParameter("asd", modifyID));
                if((int)cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Updated");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("failed");
                    this.Close();
                }
            }
        }
    }
}
