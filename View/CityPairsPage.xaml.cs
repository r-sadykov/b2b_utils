using B2B_Utils.Model;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Controls;

namespace B2B_Utils.View
{
    /// <summary>
    /// Логика взаимодействия для CityPairsPage.xaml
    /// </summary>
    public partial class CityPairsPage : Page
    {
        private OpenFileDialog _ofd;
        public CityPairsPage()
        {
            InitializeComponent();
            _ofd = null;
        }

        private void FileButton_Click(object sender, RoutedEventArgs e)
        {
            _ofd = OpenFile.GetFile();
            FileText.Text = "CityPairs Table link:\t" + _ofd.FileName;
        }

        private void ExportXMLButton_Click(object sender, RoutedEventArgs e)
        {
            ReadExcelTable readExcel = new ReadExcelTable(_ofd);
            CityPairs cityPairs= readExcel.ReadExcel();
            DateTime startTime = DateTime.UtcNow;
            double duration = (DateTime.UtcNow - startTime).TotalSeconds;
            FileText.Text += $"City Pairs was loaded in system at: {DateTime.Now.ToUniversalTime().ToString()}\tDuration: {duration.ToString():0.##} sec\n";
            RoundTripElements roundTripElements = new RoundTripElements();
            cityPairs=roundTripElements.AddRoundTripEntry(cityPairs);
            duration = (DateTime.UtcNow - startTime).TotalSeconds;
            FileText.Text += $"Added RoundTrip at: {DateTime.Now.ToUniversalTime().ToString()}\tDuration: {duration.ToString():0.##} sec\n";
            XmlMethods xml = new XmlMethods();
            xml.Serialize(cityPairs);
            duration = (DateTime.UtcNow - startTime).TotalSeconds;
            FileText.Text += $"XML formed: {DateTime.Now.ToUniversalTime().ToString()}\tDuration: {duration.ToString():0.##} sec\n";
        }

        private void ExportExcelButton_Click(object sender, RoutedEventArgs e)
        {
            ReadExcelTable readExcel = new ReadExcelTable(_ofd);
            CityPairs cityPairs = readExcel.ReadExcel();
            DateTime startTime = DateTime.UtcNow;
            double duration = (DateTime.UtcNow - startTime).TotalSeconds;
            FileText.Text += $"City Pairs was loaded in system at: {DateTime.Now.ToUniversalTime().ToString()}\tDuration: {duration.ToString():0.##} sec\n";
            RoundTripElements roundTripElements = new RoundTripElements();
            cityPairs = roundTripElements.AddRoundTripEntry(cityPairs);
            duration = (DateTime.UtcNow - startTime).TotalSeconds;
            FileText.Text += $"Added RoundTrip at: {DateTime.Now.ToUniversalTime().ToString()}\tDuration: {duration.ToString():0.##} sec\n";
            readExcel.WriteExcelFile(cityPairs);
            duration = (DateTime.UtcNow - startTime).TotalSeconds;
            FileText.Text += $"Excel formed at: {DateTime.Now.ToUniversalTime().ToString()}\tDuration: {duration.ToString():0.##} sec\n";
        }
    }
}
