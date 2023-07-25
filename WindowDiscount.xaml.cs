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
    /// Logika interakcji dla klasy WindowDiscount.xaml
    /// </summary>
    public partial class WindowDiscount : Window
    {
        MainWindow wincon = new MainWindow();
        public WindowDiscount(string id)
        {
            InitializeComponent();
            wincon.id = id;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            wincon.connectsql("server=localhost;uid=root;pwd=admin;database=ghndata;");
            wincon.conn.Open();
            decimal value;
            decimal percentValue;
            decimal currentCost;
            string sql = "SELECT cost FROM " + wincon.id + " WHERE tarrifcode = ?sel LIMIT 1";
            MySqlCommand cmd = new MySqlCommand(sql,wincon.conn);
            cmd.Parameters.Add(new MySqlParameter("sel", tarrifCode.Text));
            MySqlDataReader read = cmd.ExecuteReader();
            if (read.HasRows)
            {
                read.Read();
                currentCost = (decimal)read.GetValue(0);
                wincon.conn.Close();
                wincon.conn.Open();
                sql = "SELECT * FROM discounts WHERE discount_code=?sel LIMIT 1";
                cmd = new MySqlCommand(sql, wincon.conn);
                cmd.Parameters.Add(new MySqlParameter("sel", discountCode.Text));
                read = cmd.ExecuteReader();
                if (read.HasRows) { 
                    read.Read();
                    if (int.Parse(read["is_percent"].ToString()) == 0)
                    {
                        value = (decimal)read["discount_value"];
                        currentCost = currentCost - value;
                        Math.Round(currentCost, 2);
                    }
                    else
                    {
                        percentValue = (decimal)read["discount_value"];
                        currentCost = currentCost * ((100 - percentValue) / 100);
                        Math.Round(currentCost, 2);
                    }
                    wincon.conn.Close();
                    wincon.conn.Open();
                    sql = "UPDATE " + wincon.id + " SET cost=?c WHERE tarrifcode=?s";
                    cmd = new MySqlCommand(sql, wincon.conn);
                    cmd.Parameters.Add(new MySqlParameter("c",currentCost));
                    cmd.Parameters.Add(new MySqlParameter ("s",tarrifCode.Text));
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Zmiany zatwierdzone", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                    wincon.conn.Dispose();
                    wincon.Close();
                    this.Close();


                }
                else
                {
                    MessageBox.Show("Niepoprawny kod rabatu", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    wincon.conn.Close();
                    return;
                }

            }
            else 
            {
                MessageBox.Show("Niepoprawny kod taryfy", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                wincon.conn.Close();
                return;
            
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            wincon.Close();
        }
    }
}
