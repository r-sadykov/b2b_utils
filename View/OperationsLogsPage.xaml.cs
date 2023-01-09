using B2B_Utils.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using B2B_Utils.Model.OperationsLog;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using B2B_Utils.Model.Common;
using B2B_Utils.Model.Database;
using System.Threading.Tasks;

namespace B2B_Utils.View
{
    /// <summary>
    /// Логика взаимодействия для OperationsLogsPage.xaml
    /// </summary>
    public partial class OperationsLogsPage : Page
    {
        private OpenFileDialog _ofd;
        private readonly Agencies agencies;
        private ObservableCollection<LogInWindow> logs;
        private OperationLogModel model;
        private List<string> content;
        private readonly string connectionString;

        public ObservableCollection<LogInWindow> Logs
        {
            get { return logs; }
            set {
                logs = value;
                NotifyPropertyChanged("Logs");
            }
        }
        #region INotifyPropertyChanged Members
        public event EventHandler<PropertyChangedEventArgs> PropertyChanged;
        private void NotifyPropertyChanged(string v) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }
        #endregion
        public OperationsLogsPage()
        {
            InitializeComponent();
            _ofd = null;
            agencies = new Agencies();
            AgencyListBox.ItemsSource = agencies.AgenciesList;
            IgnoreWordsListBox.ItemsSource = agencies.IgnoreWordList;
            Logs = new ObservableCollection<LogInWindow>();
            LogGrid.ItemsSource = Logs;
            connectionString = agencies.DBConfiguration[0].DatabaseProvider + agencies.DBConfiguration[0].DatabaseSource;
                //+ agencies.DBConfiguration[0].DatabaseFile + agencies.DBConfiguration[0].DatabaseSecurity;
        }

        private void FileButton_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            string message = "";
            string elapsedTime = "";
            try
            {
                _ofd = OpenFile.GetFile();
                int numberLinesInLog = 0, errorWithLines = 0, numberLinesAfterMissingError = 0, numberLinesToAnalyse = 0;
                content = new List<string>();
                if (ExportFromDbCheckBox.IsChecked == true)
                {
                    content = WorkWithFile.ReadOperationLog(_ofd.FileName, ref numberLinesInLog, ref errorWithLines, ref numberLinesAfterMissingError, ref numberLinesToAnalyse, connectionString);
                }
                else
                {
                    bool isAlwaysNew = false;
                    if (AlwaysNewFilesCheckBox.IsChecked == true) isAlwaysNew = true;
                    content = WorkWithFile.ReadOperationLog(_ofd.FileName, ref numberLinesInLog, ref errorWithLines, ref numberLinesAfterMissingError, ref numberLinesToAnalyse, isAlwaysNew);
                }
                TimeSpan ts = stopWatch.Elapsed;
                elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                message = $"Operation Log:    <" + _ofd.FileName + $"> was uploaded in system." + Environment.NewLine + "Duration: " + elapsedTime + "." + Environment.NewLine + "Lines in Log File: " + numberLinesInLog.ToString() + Environment.NewLine + "Lines with String Length Error: " + errorWithLines.ToString() + Environment.NewLine + "Lines before checking on identities: " + numberLinesAfterMissingError.ToString() + Environment.NewLine + "Lines, ready for parsing: " + numberLinesToAnalyse.ToString();
            }
            catch
            {
                message = $"File is not loaded";
            }
            stopWatch.Stop();
            LogInWindow log = new LogInWindow() {
                Date = DateTime.Now.ToLocalTime().ToString(),
                Message = message,
                Status = $"OK"
            };
            Logs.Add(log);
        }

        private void ExportExcelButton_Click(object sender, RoutedEventArgs e)
        {
            ExportInExcel();
            //fix
        }

        public class LogInWindow
        {
            public string Date { get; set; }
            public string Message { get; set; }
            public string Status { get; set; }
        }

        private void LoadLogInSystemButton_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            if (content != null) {
                model = new OperationLogModel(content);
                var lists = AgencyListBox.SelectedItems;
                bool isCsvNeeded = false;
                if (CreateCsvCheckBox.IsChecked == true) {
                    isCsvNeeded = true;
                }
                int addContentLines = model.LoadLogContent(agencies.IgnoreWordList, isCsvNeeded, lists as List<Agencies.Agent>);
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                LogInWindow log = new LogInWindow() {
                    Date = DateTime.Now.ToLocalTime().ToString()
                };
                int linesToIgnore = content.Count - addContentLines;
                log.Message = $"OperationLog was loaded in system. Duration: " + elapsedTime + "." + Environment.NewLine + "Lines to Investigate: " + content.Count.ToString() + Environment.NewLine + "Ignored lines: " + linesToIgnore.ToString() + Environment.NewLine + "Lines to export: " + addContentLines.ToString();
                log.Status = $"OK";
                Logs.Add(log);
            } else {
                MessageBox.Show("Please load a log in system!");
                return;
            }
            if (IncludeExportInFileCheckBox.IsChecked == true) ExportInExcel();
            if (UseDbCheckBox.IsChecked == true) ExportInStorage();
        }

        private void ExportInExcel()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ReadExcelTable readExcel = new ReadExcelTable();
            if (AlwaysNewFilesCheckBox.IsChecked == true) {
                readExcel.CreateExcelTable(model, true);
            } else {
                readExcel.CreateExcelTable(model, false);
            }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            LogInWindow log = new LogInWindow() {
                Date = DateTime.Now.ToLocalTime().ToString(),
                Message = $"Excel file ready. Duration: " + elapsedTime + ".",
                Status = $"OK"
            };
            Logs.Add(log);
            content = null;
            MessageBox.Show("Excel file created");
        }

        private void ExportToDbButton_Click(object sender, RoutedEventArgs e)
        {
            ExportInStorage();
        }

        private void ExportInStorage()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            AddLogInDb.AddToDb(model, connectionString);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            LogInWindow log = new LogInWindow() {
                Date = DateTime.Now.ToLocalTime().ToString(),
                Message = $"Added in storage. Duration: " + elapsedTime + ".",
                Status = $"OK"
            };
            Logs.Add(log);
            content = null;
            MessageBox.Show("Storage ready!!!");
        }
    }
}
