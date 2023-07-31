﻿using MySqlConnector;
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
        }
        public WindowAdminAddMod(string modifyID, bool isDiscount , bool isUser)
        {
            this.modifyID = modifyID;
            this.isDiscount = isDiscount;
            this.isUser = isUser;
            InitializeComponent();
            modifyValues();

        }

    }
}
