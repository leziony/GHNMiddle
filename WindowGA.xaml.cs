using Microsoft.Win32;
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
using System.Xml;

namespace GHNMiddle
{
    /// <summary>
    /// Logika interakcji dla klasy WindowGA.xaml
    /// </summary>
    public partial class WindowGA : Window
    {
        Boolean fileAdded = false;
        public WindowGA()
        {
            InitializeComponent();
        }

        private void WindowGA_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.MainWindow.Show();
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML (*.xml)|*.xml|All files(*.*)|*.*";
            if(ofd.ShowDialog() == true )
            {
                XMLFilePath.Text = ofd.FileName;
                fileAdded=true;
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

                }
            }
        }
    }
}
