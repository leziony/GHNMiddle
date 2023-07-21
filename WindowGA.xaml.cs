using Microsoft.Win32;
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
        private void WindowGA_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.MainWindow.Show();
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
                XmlNodeList noderc = xmlDoc.SelectNodes("/inflot_export_total/services_total_list/services_list//service", nsmgr);
                int count = noderc.Count;
                for (int i = 1; i <= count; i++)
                {
                    XmlNode noder = xmlDoc.SelectSingleNode("/inflot_export_total/services_total_list/services_list/service[" + i.ToString() + "]", nsmgr);
                    row = Tab.NewRow();
                    row["Taryfa"] = noder.ChildNodes[0].InnerText;
                    row["Ilosc"] = int.Parse(noder.ChildNodes[1].InnerText);
                    row["Jednostka"] = noder.ChildNodes[2].InnerText;
                    row["Cena"] = 102.50;
                    Tab.Rows.Add(row);
                }
            }
        }
        private void ButtonCost_Click(object sender, RoutedEventArgs e)
        {
            decimal suma = 0;
            foreach (DataRow row in Tab.Rows)
            {
                sum += (decimal)row["Cena"];
            }
            Cost.Text = suma.ToString();
        }
    }
}
