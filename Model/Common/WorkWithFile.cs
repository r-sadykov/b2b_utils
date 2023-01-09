using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using System.Data;

namespace B2B_Utils.Model.Common
{
    internal static class WorkWithFile
    {
        public static string OPERATION_LOG = Environment.CurrentDirectory + "\\AppData\\Operation.log";

        public static List<string> ReadOperationLog(string fileName, ref int numberLinesInLog, ref int errorWithLines, ref int numberLinesAfterMissingError, ref int numberLinesToAnalyse, bool isAlwaysNew)
        {
            List<string> resultToSystem = new List<string>();
            IEnumerable<string> resultFromFile = File.ReadLines(fileName);
            string[] lastInvestigationLine = null;
            if (!string.IsNullOrWhiteSpace(ReadFile(OPERATION_LOG, true)[0])) {
                lastInvestigationLine = ReadFile(OPERATION_LOG, true)[0].Split(' ');
            }
            numberLinesInLog=resultFromFile.Count();
            errorWithLines = 0;
            foreach(string str in resultFromFile) {
                if (string.IsNullOrWhiteSpace(str)) continue;
                string temp = str.Substring(0, 8);

                Regex rgx = new Regex(@"^\d{2}:\d{2}:\d{2}");
                if (rgx.IsMatch(temp)) {
                    resultToSystem.Add(str);
                } else {
                    string oldString = resultToSystem.Last();
                    string newString = oldString+ str;
                    resultToSystem.Remove(oldString);
                    resultToSystem.Add(newString);
                    errorWithLines++;
                }
            }
            numberLinesAfterMissingError = resultToSystem.Count;
            List<string> finalResult = new List<string>();
            if (!isAlwaysNew) {
                foreach(string item in resultToSystem) {
                    if (lastInvestigationLine != null) {
                        DateTime dtBase = DateTime.Parse(item.Substring(0, 8));
                        DateTime dtTarget = DateTime.Parse(lastInvestigationLine[0].Substring(0, lastInvestigationLine[0].IndexOf(',')));
                        string strTarget = "timestamp=" + lastInvestigationLine[1];
                        if ((dtBase <= dtTarget) && item.Contains(strTarget)) {
                            continue;
                        }
                        finalResult.Add(item);
                    }
                }
                resultToSystem = finalResult;
                finalResult = null;
            }
            numberLinesToAnalyse = resultToSystem.Count;
            return resultToSystem;
        }

        internal static List<string> ReadOperationLog(string fileName, ref int numberLinesInLog, ref int errorWithLines, ref int numberLinesAfterMissingError, ref int numberLinesToAnalyse, string connectionString) {
            List<string> resultToSystem = new List<string>();
            IEnumerable<string> resultFromFile = File.ReadLines(fileName);
            numberLinesInLog = resultFromFile.Count();
            errorWithLines = 0;
            foreach (string str in resultFromFile) {
                if (string.IsNullOrWhiteSpace(str)) continue;
                string temp = str.Substring(0, 8);

                Regex rgx = new Regex(@"^\d{2}:\d{2}:\d{2}");
                if (rgx.IsMatch(temp)) {
                    resultToSystem.Add(str);
                } else {
                    string oldString = resultToSystem.Last();
                    string newString = oldString + str;
                    resultToSystem.Remove(oldString);
                    resultToSystem.Add(newString);
                    errorWithLines++;
                }
            }
            numberLinesAfterMissingError = resultToSystem.Count;
            List<string> finalResult = new List<string>();
            string time = "", date = "";
            string tempstr =resultToSystem.Last();
            tempstr = tempstr.Substring(tempstr.IndexOf("timestamp=")+ "timestamp=".Length, 10);
            OleDbConnection connection=null;
            try
            {
#region For MSSQL Local DB
                /*using (var connection = new SqlConnection(connectionString))
                {
                    connection.Close();
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT TOP 1 * FROM dbo.OperationLog WHERE Date='" + tempstr + "' ORDER BY Id DESC;", connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            time = reader["LoggingTime"].ToString();
                            date = reader["Date"].ToString();
                        }
                    }
                }*/
#endregion
                using(connection=new OleDbConnection(connectionString)) {
                    var adapter = new OleDbDataAdapter("SELECT * FROM OperationLog WHERE [Date]='" + tempstr + "' ORDER BY Id DESC;",connection);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    time = dt.Rows[0]["LoggingTime"].ToString();
                    date = dt.Rows[0]["Date"].ToString();
                    connection.Close();
                }

                foreach (string item in resultToSystem)
                {
                    DateTime dtBase = DateTime.Parse(item.Substring(0, 8));
                    DateTime dtTarget = DateTime.Parse(time.Substring(0, time.IndexOf(',')));
                    string strTarget = "timestamp=" + date;
                    if ((dtBase <= dtTarget) && item.Contains(strTarget))
                    {
                        continue;
                    }
                    finalResult.Add(item);
                }
                resultToSystem = finalResult;
                finalResult = null;
            }
            catch{}
            finally { connection.Close(); }
            numberLinesToAnalyse = resultToSystem.Count;
            return resultToSystem;
        }

        public static void WriteFile(string fileName, string message, bool append)
        {
            if (append) {
                File.AppendAllText(fileName, Environment.NewLine + message);
            } else {
                File.WriteAllText(fileName, message);
            }
        }

        public static bool IsFileExist(string fileName)
        {
            return File.Exists(fileName);
        }

        public static string GenerateFileName(string pattern, string extension)
        {
            return pattern + DateTime.Now.ToFileTime().ToString() + "." + extension;
        }

        public static string[] ReadFile(string fileName, bool lastString)
        {
            string[] textFile = File.ReadAllLines(fileName);
            if (lastString) {
                try {
                    return new string[] { textFile[textFile.Length - 1] };
                } catch {
                    return new string[] { null};
                }
            }
            return textFile;
        }

        public static void CreateFile(string fileName)
        {
            File.Create(fileName);
        }
    }
}
