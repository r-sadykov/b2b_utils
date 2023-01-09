using B2B_Utils.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace B2B_Utils.View
{
    /// <summary>
    /// Логика взаимодействия для SearchStat.xaml
    /// </summary>
    public partial class SearchStat : Page
    {
        private readonly Dictionary<string,OpenFileDialog> _ofd;

        public SearchStat()
        {
            InitializeComponent();
            _ofd = new Dictionary<string, OpenFileDialog>();
        }

        private void FileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file=null;
            string server=null;
            if (ProdB.IsChecked==true) server = (string)ProdB.Content;
            if (Search1.IsChecked == true) server = (string)Search1.Content;
            if (Search2.IsChecked == true) server = (string)Search2.Content;
            if (Search3.IsChecked == true) server = (string)Search3.Content;
            if (server == null)
            {
                MessageBoxResult result = MessageBox.Show("Please choose the server!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK) return;
            }
            else
            {
                if (_ofd.ContainsKey(server))
                {
                    _ofd.Remove(server);
                }
            }
            file = OpenFile.GetFile();
            _ofd.Add(server, file);
            string txt = "";
            foreach(KeyValuePair<string,OpenFileDialog> entry in _ofd)
            {
                txt += String.Format("Server:{0}\tFile:{1}\n", entry.Key, entry.Value);
            }
            FileText.Text = txt;
        }

        private void ExportExcelButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime startTime = DateTime.UtcNow;
            double duration = (DateTime.UtcNow - startTime).TotalSeconds;
            SearchStatModel model = new SearchStatModel(_ofd);
            List<B2B_Utils.Model.SearchStat> stat = model.GetStat();
            FileText.Text += $"SearchStat Log was loaded in system at: {DateTime.Now.ToUniversalTime().ToString()}\tDuration: {duration.ToString():0.##} sec\n";
            ReadExcelTable readExcel = new ReadExcelTable();
            readExcel.CreateExcelTable(stat, model.TotalGds,_ofd.Values.First<OpenFileDialog>());
            duration = (DateTime.UtcNow - startTime).TotalSeconds;
            FileText.Text += $"Excel file ready at: {DateTime.Now.ToUniversalTime().ToString()}\tDuration: {duration.ToString():0.##} sec\n";
        }
    }
}
