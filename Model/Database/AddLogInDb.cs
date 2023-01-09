using System;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel;
using System.Data.OleDb;

namespace B2B_Utils.Model.Database
{
    internal static class AddLogInDb
    {
        public static string ERROR_FILE = Environment.CurrentDirectory + "\\AppData\\Error.log";
        public static void AddToDb(OperationLogModel model, string dbConStr)
        {
            List<OperationLog> logList = new List<OperationLog>();
            int id = LoadLastId("OperationLog", dbConStr);

            foreach (var item in model.Operation) {
                id++;
                OperationLog log = new OperationLog() {
                    Id = id,
                    LoggingTime = item.TimeToLog,
                    LogStatus = item.LogStatus,
                    JBossMessage = item.JBossMesage,
                    Date = item.Operation.TimeStamp.Date,
                    Time = item.Operation.TimeStamp.Time,
                    Operation = item.Operation.Operation,
                    OperationStatus = item.Operation.OperationStatus,
                    BookingNumber = item.Operation.BookingNumber,
                    BookingStatus = item.Operation.BookingStatus,
                    ServerId = item.Operation.ServerId,
                    Tenant = item.Operation.Tenant,
                    Agency = item.Operation.Agency,
                    SalesPoint = item.Operation.SalesPoint,
                    Agent = item.Operation.Agent,
                    ServiceLocator = item.Operation.Services.ServiceLocator,
                    Gds = item.Operation.Services.Gds,
                    ChannelId = item.Operation.Services.ChannelId,
                    ServiceType = item.Operation.Services.ServiceType,
                    Route = item.Operation.Segments.Route,
                    Dates = item.Operation.Segments.Dates,
                    FlightNumbers = item.Operation.Segments.FlightNumbers,
                    Vendors = item.Operation.Segments.Vendors,
                    ProcessingTime = item.Operation.MetaData.ProcessingTime,
                    InternalErrorMessage = item.Operation.MetaData.InternalErrorMessage,
                    ExternalErrorMessage = item.Operation.MetaData.ExternalErrorMessage,
                    FlightType = item.Operation.MetaData.FlightType,
                    BoardType = item.Operation.MetaData.BoardType,
                    AdultNumber = item.Operation.MetaData.AdultNumber,
                    ChildNumber = item.Operation.MetaData.ChildNumber,
                    InfantNumber = item.Operation.MetaData.InfantNumber,
                    YouthNumber = item.Operation.MetaData.YouthNumber,
                    SeniourNumber = item.Operation.MetaData.SeniourNumber
                };
                logList.Add(log);
            }
            #region For MSSQL Local Db
            /*
            using (var connection = new SqlConnection(dbConStr)) {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try {
                    using (var sbCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction)) {
                        sbCopy.BulkCopyTimeout = 0;
                        sbCopy.BatchSize = 100;
                        sbCopy.DestinationTableName = "OperationLog";
                        var reader = logList.AsDataTable();
                        sbCopy.WriteToServer(reader);
                    }
                    transaction.Commit();
                } catch (Exception ex) {
                    WorkWithFile.WriteFile(ERROR_FILE,DateTime.Now.ToLocalTime().ToString() + " ["+ ex.Message+"]", true);
                    transaction.Rollback();
                } finally {
                    transaction.Dispose();
                    connection.Close();
                    var endTime = DateTime.Now;
                }
            }*/

            //BulkExportToAccess(logList.AsDataTable(), dbConStr, "OperationLog");
            /*DataTable dt = logList.AsDataTable();
            dt.TableName = "TargetTable";


            using (OleDbConnection connection = new OleDbConnection(dbConStr))
            {
                using(OleDbDataAdapter adapter= new OleDbDataAdapter("SELECT * FROM OperationLog;", connection))
                {
                    using(OleDbCommandBuilder builder=new OleDbCommandBuilder(adapter))
                    {
                        adapter.InsertCommand = builder.GetInsertCommand(true);
                        adapter.AcceptChangesDuringFill = true;
                        adapter.Update(dt);
                    }
                }               
            }
            */
#endregion
            InsertDataInAccess("OperationLog", dbConStr, logList.AsDataTable());
        }

        public static DataTable LoadDataTable(string databaseTableName, string connectionString)
        {
            DataTable dt = new DataTable();
            string SQL = "SELECT * FROM " + databaseTableName + ";";
            OleDbConnection connection = new OleDbConnection(connectionString);
            OleDbDataAdapter adapter = new OleDbDataAdapter(SQL, connection);
            connection.Open();
            adapter.Fill(dt);
            connection.Close();
            return dt;
        }

        public static int LoadLastId(string databaseTableName, string connectionString)
        {
            string SQL = "SELECT TOP 1 Id FROM " + databaseTableName + " ORDER BY Id DESC;";
            OleDbConnection connection = new OleDbConnection(connectionString);
            OleDbCommand cmd = new OleDbCommand(SQL, connection);
            connection.Open();
            cmd.ExecuteNonQuery();
            int id = (int)cmd.ExecuteScalar();
            connection.Close();
            return id;
        }

        public static void InsertDataInAccess(string databaseTableName, string connectionString, DataTable dataTable)
        {
            string SQL = "SELECT Id, LoggingTime, LogStatus, JBossMessage, Date, Time, Operation, OperationStatus, BookingNumber, BookingStatus, ServerId, Tenant, Agency, SalesPoint, Agent, ServiceLocator, Gds, ChannelId, ServiceType, Route, Dates, FlightNumbers, Vendors, ProcessingTime, InternalErrorMessage, ExternalErrorMessage, FlightType, BoardType, AdultNumber, ChildNumber, InfantNumber, YouthNumber, SeniourNumber FROM " + databaseTableName+";";
            string INSERT = "INSERT INTO "+databaseTableName+"(Id, LoggingTime, LogStatus, JBossMessage, [Date], [Time], Operation, OperationStatus, BookingNumber, BookingStatus, ServerId, Tenant, Agency, SalesPoint, Agent, ServiceLocator, Gds, ChannelId, ServiceType, Route, Dates, FlightNumbers, Vendors, ProcessingTime, InternalErrorMessage, ExternalErrorMessage, FlightType, BoardType, AdultNumber, ChildNumber, InfantNumber, YouthNumber, SeniourNumber) " + "VALUES (@Id, @LoggingTime, @LogStatus, @JBossMessage, @Date, @Time, @Operation, @OperationStatus, @BookingNumber, @BookingStatus, @ServerId, @Tenant, @Agency, @SalesPoint, @Agent, @ServiceLocator, @Gds, @ChannelId, @ServiceType, @Route, @Dates, @FlightNumbers, @Vendors, @ProcessingTime, @InternalErrorMessage, @ExternalErrorMessage, @FlightType, @BoardType, @AdultNumber, @ChildNumber, @InfantNumber, @YouthNumber, @SeniourNumber);";
            OleDbConnection OleConn = new OleDbConnection(connectionString);
            OleDbDataAdapter OleAdp = new OleDbDataAdapter(SQL, OleConn) {
                InsertCommand = new OleDbCommand(INSERT)
            };
            OleAdp.InsertCommand.Parameters.Add("@Id", OleDbType.Integer,Int32.MaxValue, "Id");
            OleAdp.InsertCommand.Parameters.Add("@LoggingTime", OleDbType.VarWChar,150, "LoggingTime");
            OleAdp.InsertCommand.Parameters.Add("@LogStatus", OleDbType.VarWChar, 150, "LogStatus");
            OleAdp.InsertCommand.Parameters.Add("@JBossMessage", OleDbType.VarWChar, 150, "JBossMessage");
            OleAdp.InsertCommand.Parameters.Add("@Date", OleDbType.VarWChar, 150, "Date");
            OleAdp.InsertCommand.Parameters.Add("@Time", OleDbType.VarWChar, 150, "Time");
            OleAdp.InsertCommand.Parameters.Add("@Operation", OleDbType.VarWChar, 150, "Operation");
            OleAdp.InsertCommand.Parameters.Add("@OperationStatus", OleDbType.VarWChar, 150, "OperationStatus");
            OleAdp.InsertCommand.Parameters.Add("@BookingNumber", OleDbType.VarWChar, 150, "BookingNumber");
            OleAdp.InsertCommand.Parameters.Add("@BookingStatus", OleDbType.VarWChar, 150, "BookingStatus");
            OleAdp.InsertCommand.Parameters.Add("@ServerId", OleDbType.VarWChar, 150, "ServerId");
            OleAdp.InsertCommand.Parameters.Add("@Tenant", OleDbType.VarWChar, 150, "Tenant");
            OleAdp.InsertCommand.Parameters.Add("@Agency", OleDbType.VarWChar, 150, "Agency");
            OleAdp.InsertCommand.Parameters.Add("@SalesPoint", OleDbType.VarWChar, 150, "SalesPoint");
            OleAdp.InsertCommand.Parameters.Add("@Agent", OleDbType.VarWChar, 150, "Agent");
            OleAdp.InsertCommand.Parameters.Add("@ServiceLocator", OleDbType.VarWChar, 150, "ServiceLocator");
            OleAdp.InsertCommand.Parameters.Add("@Gds", OleDbType.VarWChar, 255, "Gds");
            OleAdp.InsertCommand.Parameters.Add("@ChannelId", OleDbType.VarWChar, 255, "ChannelId");
            OleAdp.InsertCommand.Parameters.Add("@ServiceType", OleDbType.VarWChar, 150, "ServiceType");
            OleAdp.InsertCommand.Parameters.Add("@Route", OleDbType.VarWChar, 255, "Route");
            OleAdp.InsertCommand.Parameters.Add("@Dates", OleDbType.VarWChar, 255, "Dates");
            OleAdp.InsertCommand.Parameters.Add("@FlightNumbers", OleDbType.VarWChar, 150, "FlightNumbers");
            OleAdp.InsertCommand.Parameters.Add("@Vendors", OleDbType.VarWChar, 150, "Vendors");
            OleAdp.InsertCommand.Parameters.Add("@ProcessingTime", OleDbType.VarWChar, 150, "ProcessingTime");
            OleAdp.InsertCommand.Parameters.Add("@InternalErrorMessage", OleDbType.VarWChar, 255, "InternalErrorMessage");
            OleAdp.InsertCommand.Parameters.Add("@ExternalErrorMessage", OleDbType.VarWChar, 255, "ExternalErrorMessage");
            OleAdp.InsertCommand.Parameters.Add("@FlightType", OleDbType.VarWChar, 150, "FlightType");
            OleAdp.InsertCommand.Parameters.Add("@BoardType", OleDbType.VarWChar, 150, "BoardType");
            OleAdp.InsertCommand.Parameters.Add("@AdultNumber", OleDbType.Integer, 8, "AdultNumber");
            OleAdp.InsertCommand.Parameters.Add("@ChildNumber", OleDbType.Integer, 8, "ChildNumber");
            OleAdp.InsertCommand.Parameters.Add("@InfantNumber", OleDbType.Integer, 8, "InfantNumber");
            OleAdp.InsertCommand.Parameters.Add("@YouthNumber", OleDbType.Integer, 8, "YouthNumber");
            OleAdp.InsertCommand.Parameters.Add("@SeniourNumber", OleDbType.Integer, 8, "SeniourNumber");
            OleAdp.InsertCommand.Connection = OleConn;
            OleAdp.InsertCommand.Connection.Open();
            OleAdp.Update(dataTable);
            OleAdp.InsertCommand.Parameters.Clear();
            OleAdp.InsertCommand.Connection.Close();
        }
    }

    public static class IEnumerableExtensions
    {
        public static DataTable AsDataTable<T>(this IEnumerable<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data) {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
    }
}

