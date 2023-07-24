using Microsoft.Win32;
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
using System.Xml;

namespace GHNMiddle
{
    /// <summary>
    /// Logika interakcji dla klasy WindowGA.xaml
    /// </summary>
    public partial class WindowGA : Window
    {
        Boolean fileAdded = false;
        System.Data.DataTable Tab = new DataTable();
        DataColumn column;
        DataRow row;
        MainWindow windowconnect = new MainWindow();

        public void TableInit()
        {
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Taryfa";
            Tab.Columns.Add(column);
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "Ilosc";
            Tab.Columns.Add(column);
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Jednostka";
            Tab.Columns.Add(column);
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Decimal");
            column.ColumnName = "Cena";
            Tab.Columns.Add(column);
            Table.DataContext = Tab;
        }
        public WindowGA()
        {
            InitializeComponent();
            TableInit();
        }
        public WindowGA(string s)
        {
            InitializeComponent();
            TableInit();
            windowconnect.id = s;
        }
        public void CostCalc()
        {
            if (fileAdded == false)
            {
                MessageBox.Show("No file was added!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            decimal suma = 0;
            foreach (DataRow row in Tab.Rows)
            {
                suma += (decimal)row["Cena"];
            }
            if(Decimal.Parse(Discount.Text) != 0)
            {
                suma = suma * ((100-Decimal.Parse(Discount.Text)) / 100);
            }
            suma = Math.Round(suma,2);
            Cost.Text = suma.ToString();
        }
        private void WindowGA_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var ifCancel = MessageBox.Show("Nie wyeksportowano danych. Po wyjściu z okna dane zostaną UTRACONE. \n Czy nadal chcesz wyjść z programu?", "Wyjscie", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if(ifCancel != MessageBoxResult.OK) 
            {
                e.Cancel = true;
            }
            else
            {
                windowconnect.connectsql("server=localhost;uid=root;pwd=admin;database=ghndata;");
                windowconnect.conn.Open();
                string sql = "DROP TABLE IF EXISTS "+ windowconnect.id + ";";
                MySqlCommand cmd = new MySqlCommand(sql, windowconnect.conn);

                cmd.ExecuteNonQuery();
                windowconnect.conn.Dispose();
                Application.Current.MainWindow.Show();
                windowconnect.changeId("N/A");
                windowconnect.Close();
            }    

        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML (*.xml)|*.xml|All files(*.*)|*.*";
            if (ofd.ShowDialog() == true)
            {
                XMLFilePath.Text = ofd.FileName;
                fileAdded = true;
            }
        }

        private void ButtonLoadInfo_Click(object sender, RoutedEventArgs e)
        {
            if (fileAdded == false)
            {
                MessageBox.Show("No file was added!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                String filename = XMLFilePath.Text;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filename);
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                XmlNodeList noder = xmlDoc.SelectNodes("/inflot_export_total/commercial_client", nsmgr);
                foreach (XmlNode xn in noder)
                {
                    ClientID.Text = xn["commercial_client_id"].InnerText;
                    CompanyName.Text = xn["name"].InnerText;
                    TaxpayerID.Text = xn["nip"].InnerText;
                    Street.Text = xn["street"].InnerText;
                    HouseNr.Text = xn["house_number"].InnerText;
                    FlatNr.Text = xn["flat_number"].InnerText;
                    PostalCode.Text = xn["postal_code"].InnerText;
                    City.Text = xn["city"].InnerText;
                    Country.Text = xn["country"].InnerText;
                    CountryCode.Text = xn["country_iso2"].InnerText;

                }
            }
        }

        private void ButtonLoadTarrif_Click(object sender, RoutedEventArgs e)
        {
            Tab.Rows.Clear();
            Cost.Text = "N/A";
            if (fileAdded == false)
            {
                MessageBox.Show("No file was added!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                windowconnect.connectsql("server=localhost;uid=root;pwd=admin;database=ghndata;");
                String filename = XMLFilePath.Text;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filename);
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                XmlNodeList noderc = xmlDoc.SelectNodes("/inflot_export_total/services_total_list/services_list//service", nsmgr);
                int count = noderc.Count;
                for (int i = 1; i <= count; i++)
                {
                    windowconnect.conn.Open();
                    XmlNode noder = xmlDoc.SelectSingleNode("/inflot_export_total/services_total_list/services_list/service[" + i.ToString() + "]", nsmgr);
                    row = Tab.NewRow();
                    row["Taryfa"] = noder.ChildNodes[0].InnerText;
                    int value = int.Parse(noder.ChildNodes[1].InnerText);
                    row["Ilosc"] = value;
                    row["Jednostka"] = noder.ChildNodes[2].InnerText;
                    string sql = "SELECT cost FROM tarrif_code WHERE tarrifID=?tarrif LIMIT 1";
                    MySqlCommand cmd = new MySqlCommand(sql,windowconnect.conn);
                    cmd.Parameters.Add(new MySqlParameter("tarrif", noder.ChildNodes[0].InnerText));
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        decimal result = (decimal)reader.GetValue(0);
                        row["Cena"] = result * value;
                    }
                    else
                    {
                        row["Cena"] = 102.50;
                    }
                    Tab.Rows.Add(row);
                    windowconnect.conn.Close();
                }
                CostCalc();
            }
        }
        private void ButtonCost_Click(object sender, RoutedEventArgs e)
        {
            CostCalc();
        }

        private void DiscountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (fileAdded != false)
            {
                CostCalc();
            }
        }

        private void Discount_KeyDown(object sender, KeyEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(Discount.Text, "[^0-9]"))
            {
                Discount.Text = Discount.Text.Remove(Discount.Text.Length - 1,1);
            }
        }
    }           
}
