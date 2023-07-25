using Microsoft.Win32;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using System.Windows.Threading;
using System.Xml;

namespace GHNMiddle
{
    /// <summary>
    /// Logika interakcji dla klasy WindowGA.xaml
    /// </summary>
    public partial class WindowGA : Window
    {
        Boolean fileAdded = false;
        bool first_launch = false;
        bool checkup = false;
        bool exportblocker= false;
        bool exportComplete = false;
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
            windowconnect.connectsql("server=localhost;uid=root;pwd=admin;database=ghndata;");
        }
        public WindowGA(string s)
        {
            InitializeComponent();
            TableInit();
            windowconnect.connectsql("server=localhost;uid=root;pwd=admin;database=ghndata;");
            windowconnect.id = s;
        }
        public void SQLupdate()
        {
            windowconnect.conn.Open();
            string sql = "SELECT * FROM " + windowconnect.id;
            MySqlCommand cmd = new MySqlCommand(sql, windowconnect.conn);
            MySqlDataReader read = cmd.ExecuteReader();
            if(read.HasRows)
            {
                Tab.Rows.Clear();
                while (read.Read())
                {
                    row = Tab.NewRow();
                    row["Taryfa"] = read["tarrifcode"];
                    row["Ilosc"] = read["ammount"];
                    row["Jednostka"] = read["unit"];
                    row["Cena"] = read["cost"];
                    Tab.Rows.Add(row);
                }
            }
            else if (checkup == false)
            {
                MessageBox.Show("Your database was already deleted. Data is irrecoverable.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                exportblocker = true;
                windowconnect.conn.Close();
                return;
            }
            else
            {
                windowconnect.conn.Close();
                return;
            }
            windowconnect.conn.Close();
            fileAdded = true;
            CostCalc();
        }

        public void CostCalc()
        {
            if (fileAdded == false)
            {
                MessageBox.Show("No file was added!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            decimal suma = 0;
            if(Discount.Text.Length == 0)
            {
                Discount.Text = "0";
            }
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

        public void TableDrop()
        {
            windowconnect.connectsql("server=localhost;uid=root;pwd=admin;database=ghndata;");
            windowconnect.conn.Open();
            string sql = "DROP TABLE IF EXISTS " + windowconnect.id + ";";
            MySqlCommand cmd = new MySqlCommand(sql, windowconnect.conn);
            cmd.ExecuteNonQuery();
            windowconnect.conn.Dispose();
            Application.Current.MainWindow.Show();
            windowconnect.changeId("N/A");
            windowconnect.Close();
        }
        private void WindowGA_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(exportComplete == false) {
                var ifCancel = MessageBox.Show("Nie wyeksportowano danych. Po wyjściu z okna dane zostaną UTRACONE. \n Czy nadal chcesz wyjść z programu?", "Wyjscie", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (ifCancel != MessageBoxResult.OK)
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    TableDrop();
                }
            }
            else
            {
                TableDrop();
            }    

        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML (*.xml)|*.xml";
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
                return;
            }
            else if (XMLFilePath.Text == "Recovered from SQL")
            {
                MessageBox.Show("Aktualnie jesteś w trybie post-recovery. \n Musisz znaleść orginalny plik XML by wprowadić dane klienta", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                exportComplete = false;
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
            if (fileAdded == false || XMLFilePath.Text == "No XML File")
            {
                MessageBox.Show("No file was added!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                windowconnect.connectsql("server=localhost;uid=root;pwd=admin;database=ghndata;");
                String filename = XMLFilePath.Text;
                try
                {
                    XmlDocument xmlDoct = new XmlDocument();
                    xmlDoct.Load(filename);
                }
                catch (XmlException ex) {
                MessageBox.Show ("This is not a valid XML File. Try again.","Error",MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                catch (FileNotFoundException ex)
                {
                    MessageBox.Show("This is not a valid XML File. Try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filename);
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                XmlNodeList noderc = xmlDoc.SelectNodes("/inflot_export_total/services_total_list/services_list//service", nsmgr);
                int count = noderc.Count;
                decimal result = 0;
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
                        result = (decimal)reader.GetValue(0);
                        row["Cena"] = result * value;
                    }
                    else
                    {
                        result = (decimal)124.50;
                        row["Cena"] = 124.50 * value;
                    }
                    Tab.Rows.Add(row);
                    windowconnect.conn.Close();
                    windowconnect.conn.Open();
                    sql = "INSERT INTO " + windowconnect.id + " VALUES (?a,?b,?c,?d);";
                    cmd = new MySqlCommand(sql,windowconnect.conn);
                    cmd.Parameters.Add(new MySqlParameter("a", noder.ChildNodes[0].InnerText));
                    cmd.Parameters.Add(new MySqlParameter("b", int.Parse(noder.ChildNodes[1].InnerText)));
                    cmd.Parameters.Add(new MySqlParameter("c", noder.ChildNodes[2].InnerText));
                    cmd.Parameters.Add(new MySqlParameter("d", result*value));
                    cmd.ExecuteNonQuery();
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

        private void ButtonDiscount_Click(object sender, RoutedEventArgs e)
        {
            WindowDiscount dis = new WindowDiscount(windowconnect.id);
            dis.Show();

        }

        private void ButtonRecover_Click(object sender, RoutedEventArgs e)
        {
            if (fileAdded== true) 
            {
                var confirm = MessageBox.Show("Odzyskiwanie danych oznacza utrate danych.\nCzy na pewno chcesz odzyskać dane?", "Odzysk", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if(confirm == MessageBoxResult.No) {
                    return;
                }
                fileAdded = false;
            }
            XMLFilePath.Text = "Recovered from SQL";
            SQLupdate();
        }

        private void ButtonExport_Click(object sender, RoutedEventArgs e)
        {
            exportblocker = false;
            SQLupdate();
            if(exportblocker == true)
            {
                MessageBox.Show("Export was blocked due to an error", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                exportblocker= false;
                return;
            }
            windowconnect.conn.Open();
            /*String filename = XMLFilePath.Text;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filename);
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
            XmlNodeList noder = xmlDoc.SelectNodes("/inflot_export_total/commercial_client", nsmgr);*/
            string sql = "SELECT * FROM " + windowconnect.id;
            MySqlCommand cmd = new MySqlCommand(sql,windowconnect.conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            using (StreamWriter w = new StreamWriter("export.txt"))
            {
                while (reader.Read())
                {
                    w.Write(reader["tarrifcode"]);
                    w.Write(reader["ammount"].ToString());
                    w.Write(reader["unit"]);
                    w.WriteLine(reader["cost"]);

                }
                    w.WriteLine(ClientID.Text);
                    w.WriteLine(CompanyName.Text);
                    w.WriteLine(TaxpayerID.Text);
                    w.WriteLine(Street.Text);
                    w.WriteLine(HouseNr.Text);
                    w.WriteLine(FlatNr.Text);
                    w.WriteLine(PostalCode.Text);
                    w.WriteLine(City.Text);
                    w.WriteLine(Country.Text);
                    w.WriteLine(CountryCode.Text);
                w.Close();
             
            }
            windowconnect.conn.Close();
            MessageBox.Show("Dane zostały exportowane","Export complete",MessageBoxButton.OK,MessageBoxImage.Exclamation);
            exportComplete = true;

        }

        private void Window_Activated(object sender, EventArgs e)
        {
            if(first_launch == false)
            {
                first_launch = true;
                return;
            }
            else if (fileAdded == false) { return; }
            else
            {
                checkup = true;
                SQLupdate();
                checkup = false;
            }

        }
    }           
}
