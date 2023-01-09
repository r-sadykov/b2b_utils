using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using Microsoft.Office.Interop.Excel;
using Application = Microsoft.Office.Interop.Excel.Application;
using B2B_Utils.Model.Common;
using System.ComponentModel;

namespace B2B_Utils.Model
{
    internal class ReadExcelTable
    {
        private readonly CityPairs _pairs;
        private CityPair _city;
        private readonly string _connection;
        public string ERROR_FILE = Environment.CurrentDirectory + "\\AppData\\Error.log";
        public string OUTPUT_DIRECTORY = Environment.CurrentDirectory + "\\AppData\\Output\\";
        public string OUTPUT_LOG = Environment.CurrentDirectory + "\\AppData\\Output.log";
        public string OPERATION_LOG= Environment.CurrentDirectory + "\\AppData\\Operation.log";

        public ReadExcelTable(OpenFileDialog file)
        {
            _pairs = new CityPairs();
            _connection = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + file.FileName +
                          ";Extended Properties='Excel 12.0 Xml;HDR=YES;'";
        }

        public ReadExcelTable()
        {

        }

        public CityPairs ReadExcel()
        {
            using (OleDbConnection con = new OleDbConnection(_connection))
            {
                con.Open();
                OleDbCommand command = new OleDbCommand("select * from [Tabelle1$]", con);
                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        _city = new CityPair() {
                            Carrier = ((string)dr[0]).Replace("'", ""),
                            Departure = ((string)dr[1]).Replace("'", ""),
                            DepartureLand = ((string)dr[3]).Replace("'", ""),
                            Destination = ((string)dr[2]).Replace("'", ""),
                            DestinationLand = ((string)dr[4]).Replace("'", ""),
                            Land = ((string)dr[5]).Replace("'", "")
                        };
                        _pairs.Cities.Add(_city);
                        _city = null;
                    }
                }
                con.Close();
            }

            return _pairs;
        }

        public void WriteExcelFile(CityPairs pairs)
        {
            using (OleDbConnection conn = new OleDbConnection(_connection))
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand() {
                    Connection = conn
                };
                string commandText;
                try
                {
                    const string createTable =
                        "CREATE TABLE [CityPairs] ('carrier' VARCHAR, 'dep' VARCHAR, 'dst' VARCHAR, 'dep_iso_land' VARCHAR, 'dst_iso_land' VARCHAR, 'Land' VARCHAR);";
                    commandText = createTable;
                    cmd.CommandText = commandText;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBoxResult result = MessageBox.Show(e.Message + "\nDo you want delete old entries?", "Error",
                        MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        const string createTable =
                            "DROP TABLE [CityPairs]; CREATE TABLE [CityPairs] ('carrier' VARCHAR, 'dep' VARCHAR, 'dst' VARCHAR, 'dep_iso_land' VARCHAR, 'dst_iso_land' VARCHAR, 'Land' VARCHAR);";
                        commandText = createTable;
                        cmd.CommandText = commandText;
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        return;
                    }
                }

                string contentTable = "";
                foreach (CityPair pair in pairs.Cities)
                {
                    contentTable =
                        "INSERT INTO [CityPairs]('carrier', 'dep', 'dst', 'dep_iso_land', 'dst_iso_land', 'Land') VALUES('" +
                        pair.Carrier + "','" + pair.Departure + "','" + pair.Destination + "','" + pair.DepartureLand +
                        "','" + pair.DestinationLand + "','" + pair.Land + "');";
                    commandText = contentTable;
                    cmd.CommandText = commandText;
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
            }
        }

        internal void CreateExcelTable(List<Model.SearchStat> stat, List<string> Gds, OpenFileDialog file)
        {
            Application xlApplication = new Application();
            object misValue = System.Reflection.Missing.Value;

            string fileExcel = Path.GetDirectoryName(file.FileName) + "\\SearchStatLog_" + DateTime.Now.ToFileTime().ToString() + ".xlsx";

            var xlWorkBook = xlApplication.Workbooks.Add(misValue);
            var xlWorkSheet = (Worksheet) xlWorkBook.Worksheets.Item[1];

            long row = 1;
            foreach (SearchStat st in stat)
            {
                long col = 1;
                xlWorkSheet.Cells[row, col] = st.Date;
                col++;
                xlWorkSheet.Cells[row, col] = st.Time;
                col++;
                xlWorkSheet.Cells[row, col] = st.Gmt;
                col++;
                xlWorkSheet.Cells[row, col] = st.Server;
                col++;
                xlWorkSheet.Cells[row, col] = st.ServerId;
                col++;
                xlWorkSheet.Cells[row, col] = st.Tenant;
                col++;
                xlWorkSheet.Cells[row, col] = st.AgencyNumber;
                col++;
                xlWorkSheet.Cells[row, col] = st.SP;
                col++;
                xlWorkSheet.Cells[row, col] = st.Agent;
                col++;
                xlWorkSheet.Cells[row, col] = st.Routes;
                col++;
                xlWorkSheet.Cells[row, col] = st.Pax;
                col++;
                xlWorkSheet.Cells[row, col] = st.Class;
                col++;
                xlWorkSheet.Cells[row, col] = st.TotalFlightsCount;
                col++;
                xlWorkSheet.Cells[row, col] = st.TotalFlightsAfterFilter;
                col++;
                xlWorkSheet.Cells[row, col] = st.TotalRespTime;
                col++;
                xlWorkSheet.Cells[row, col] = st.TotalPreProcessingTime;
                col++;
                xlWorkSheet.Cells[row, col] = st.TotalSearchTime;
                col++;
                xlWorkSheet.Cells[row, col] = st.TotalPostProcessingTime;
                col++;
                foreach(SearchStatGds gds in st.GdsList)
                {
                    foreach(string g in Gds)
                    {
                        if (gds.Gds.Equals(g))
                        {
                            xlWorkSheet.Cells[row, col] = gds.Gds;
                            col++;
                            xlWorkSheet.Cells[row, col] = gds.FlightCount;
                            col++;
                            xlWorkSheet.Cells[row, col] = gds.RespTime;
                            col++;
                            xlWorkSheet.Cells[row, col] = gds.BlackWhiteListHit;
                            col++;
                            xlWorkSheet.Cells[row, col] = gds.Error;
                            col++;
                            xlWorkSheet.Cells[row, col] = gds.CacheHit;
                            col++;
                        }
                        else
                        {
                            xlWorkSheet.Cells[row, col] = "";
                            col++;
                            xlWorkSheet.Cells[row, col] = "";
                            col++;
                            xlWorkSheet.Cells[row, col] = "";
                            col++;
                            xlWorkSheet.Cells[row, col] = "";
                            col++;
                            xlWorkSheet.Cells[row, col] = "";
                            col++;
                            xlWorkSheet.Cells[row, col] = "";
                            col++;
                        }
                    }
                }
                row++;
            }

            xlWorkBook.SaveAs(fileExcel, XlFileFormat.xlWorkbookDefault, misValue, misValue, misValue, misValue, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApplication.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApplication);

            MessageBox.Show("Excel file created , you can find the file"+fileExcel);
        }

        public void CreateExcelTable(OperationLogModel model, bool isAlwaysNew)
        {
            string fileName;
            if (isAlwaysNew | string.IsNullOrWhiteSpace(WorkWithFile.ReadFile(OUTPUT_LOG, true)[0])) {
                fileName = WorkWithFile.GenerateFileName("OL", "xlsx");
                WorkWithFile.WriteFile(OUTPUT_LOG, fileName.Substring(0, fileName.IndexOf('.')), true);
            } else {
                fileName = WorkWithFile.ReadFile(OUTPUT_LOG, true)[0] + ".xlsx";
            }
            string fileExcel = OUTPUT_DIRECTORY + fileName;

            Application xlApplication = new Application() {
                Visible = false
            };
            object misValue = System.Reflection.Missing.Value;
            Workbooks xlWorkBooks = xlApplication.Workbooks;
            Workbook xlWorkBook; //= xlWorkBooks.Item[1];
            Worksheet xlWorkSheet; //= (Worksheet)xlWorkBook.Worksheets.Item[1];
            int row = 1;
            if (!isAlwaysNew & File.Exists(fileExcel)) {
                xlWorkBook=xlWorkBooks.Open(fileExcel, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.Item[1];
                var last = xlWorkSheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell, Type.Missing);
                row = last.Row+1;
                AddRowsInExcel(ref xlWorkSheet, row, model);
                try {
                    SaveExcel(ref xlWorkBook, misValue);
                } catch {
                    fileName= WorkWithFile.GenerateFileName("OL", "xlsx");
                    WorkWithFile.WriteFile(OUTPUT_LOG, fileName.Substring(0, fileName.IndexOf('.')), true);
                    SaveAsExcel(ref xlWorkBook, OUTPUT_DIRECTORY + fileName, misValue);
                }
            } else {
                xlWorkBook = xlWorkBooks.Add(misValue);
                xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.Item[1];
                AddHeaderInExcel(ref xlWorkSheet, row);
                row++;
                AddRowsInExcel(ref xlWorkSheet, row, model);
                SaveAsExcel(ref xlWorkBook, OUTPUT_DIRECTORY+ fileName, misValue);
            }
            xlApplication.Quit();
            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApplication);
            try {
                WorkWithFile.WriteFile(OPERATION_LOG, model.Operation.LastOrDefault()?.TimeToLog + " " + model.Operation.LastOrDefault()?.Operation.TimeStamp.Date, true);
            } catch {
                MessageBox.Show("Nothing new!!!");
            }
        }

        private void SaveAsExcel(ref Workbook xlWorkBook, string fileName, object misValue)
        {
            xlWorkBook.SaveAs(fileName, XlFileFormat.xlWorkbookDefault, misValue, misValue, misValue, misValue, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
        }

        private void SaveExcel(ref Workbook xlWorkBook, object misValue)
        {
            xlWorkBook.Save();
            xlWorkBook.Close(true, misValue, misValue);
        }

        private void AddRowsInExcel(ref Worksheet xlWorkSheet, int row, OperationLogModel model) {
            Range xlCellRange = xlWorkSheet.Range[xlWorkSheet.Cells[row, 3], xlWorkSheet.Cells[row+model.Operation.Count-1, 28]];
            object[,] values = new object[model.Operation.Count, 26];
            int cols, rows = 0;
            foreach (OperationLog operationLog in model.Operation) {
                cols = 0;
                values[rows, cols] = operationLog.TimeToLog;
                cols++;
                //xlWorkSheet.Cells[row, col] = operationLog.LogStatus;
                //col++;
                //xlWorkSheet.Cells[row, col] = operationLog.JBossMesage;
                //col++;
                values[rows, cols] = operationLog.Operation.TimeStamp.Date;
                cols++;
                //xlWorkSheet.Cells[row, col] = operationLog.Operation.TimeStamp.Time;
                //col++;
                values[rows, cols] = operationLog.Operation.Operation;
                cols++;
                values[rows, cols] = operationLog.Operation.OperationStatus;
                cols++;
                values[rows, cols] = operationLog.Operation.BookingNumber;
                cols++;
                values[rows, cols] = operationLog.Operation.BookingStatus;
                cols++;
                //xlWorkSheet.Cells[row, col] = operationLog.Operation.ServerId;
                //col++;
                //xlWorkSheet.Cells[row, col] = operationLog.Operation.Tenant;
                //col++;
                values[rows, cols] = operationLog.Operation.Agency;
                cols++;
                values[rows, cols] = operationLog.Operation.SalesPoint;
                cols++;
                values[rows, cols] = operationLog.Operation.Agent;
                cols++;
                values[rows, cols] = operationLog.Operation.Services.ServiceLocator;
                cols++;
                values[rows, cols] = operationLog.Operation.Services.Gds;
                cols++;
                values[rows, cols] = operationLog.Operation.Services.ChannelId;
                cols++;
                //xlWorkSheet.Cells[row, col] = operationLog.Operation.Services.ServiceType;
                //col++;
                values[rows, cols] = operationLog.Operation.Segments.Route;
                cols++;
                values[rows, cols] = operationLog.Operation.Segments.Dates;
                cols++;
                values[rows, cols] = operationLog.Operation.Segments.FlightNumbers;
                cols++;
                values[rows, cols] = operationLog.Operation.Segments.Vendors;
                cols++;
                values[rows, cols] = operationLog.Operation.MetaData.ProcessingTime;
                cols++;
                values[rows, cols] = operationLog.Operation.MetaData.InternalErrorMessage;
                cols++;
                values[rows, cols] = operationLog.Operation.MetaData.ExternalErrorMessage;
                cols++;
                values[rows, cols] = operationLog.Operation.MetaData.FlightType;
                cols++;
                values[rows, cols] = operationLog.Operation.MetaData.BoardType;
                cols++;
                values[rows, cols] = operationLog.Operation.MetaData.AdultNumber;
                cols++;
                values[rows, cols] = operationLog.Operation.MetaData.ChildNumber;
                cols++;
                values[rows, cols] = operationLog.Operation.MetaData.InfantNumber;
                cols++;
                values[rows, cols] = operationLog.Operation.MetaData.YouthNumber;
                cols++;
                values[rows, cols] = operationLog.Operation.MetaData.SeniourNumber;

                rows++;
            }
            xlCellRange.Value2 = values;
        }

        private void AddHeaderInExcel(ref Worksheet xlWorkSheet, int row) {
            int col=3;
            xlWorkSheet.Cells[row, col] = "LoggingTime";
            col++;
            //xlWorkSheet.Cells[row, col] = "LogStatus";
            //col++;
            //xlWorkSheet.Cells[row, col] = "JBossMesage";
            //col++;
            xlWorkSheet.Cells[row, col] = "Date";
            col++;
            //xlWorkSheet.Cells[row, col] = "Time";
            //col++;
            xlWorkSheet.Cells[row, col] = "Operation";
            col++;
            xlWorkSheet.Cells[row, col] = "OperationStatus";
            col++;
            xlWorkSheet.Cells[row, col] = "BookingNumber";
            col++;
            xlWorkSheet.Cells[row, col] = "BookingStatus";
            col++;
            //xlWorkSheet.Cells[row, col] = "ServerId";
            //col++;
            //xlWorkSheet.Cells[row, col] = "Tenant";
            //col++;
            xlWorkSheet.Cells[row, col] = "Agency";
            col++;
            xlWorkSheet.Cells[row, col] = "SalesPoint";
            col++;
            xlWorkSheet.Cells[row, col] = "Agent";
            col++;
            xlWorkSheet.Cells[row, col] = "ServiceLocator";
            col++;
            xlWorkSheet.Cells[row, col] = "Gds";
            col++;
            xlWorkSheet.Cells[row, col] = "ChannelId";
            col++;
            //xlWorkSheet.Cells[row, col] = "ServiceType";
            //col++;
            xlWorkSheet.Cells[row, col] = "Route";
            col++;
            xlWorkSheet.Cells[row, col] = "Dates";
            col++;
            xlWorkSheet.Cells[row, col] = "FlightNumbers";
            col++;
            xlWorkSheet.Cells[row, col] = "Vendors";
            col++;
            xlWorkSheet.Cells[row, col] = "ProcessingTime";
            col++;
            xlWorkSheet.Cells[row, col] = "InternalErrorMessage";
            col++;
            xlWorkSheet.Cells[row, col] = "ExternalErrorMessage";
            col++;
            xlWorkSheet.Cells[row, col] = "FlightType";
            col++;
            xlWorkSheet.Cells[row, col] = "BoardType";
            col++;
            xlWorkSheet.Cells[row, col] = "AdultNumber";
            col++;
            xlWorkSheet.Cells[row, col] = "ChildNumber";
            col++;
            xlWorkSheet.Cells[row, col] = "InfantNumber";
            col++;
            xlWorkSheet.Cells[row, col] = "YouthNumber";
            col++;
            xlWorkSheet.Cells[row, col] = "SeniourNumber";
        }
    }

    public static class IEnumerableExtensions
    {
        public static System.Data.DataTable AsDataTable<T>(this IEnumerable<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            var table = new System.Data.DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data) {
                System.Data.DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
    }
}
