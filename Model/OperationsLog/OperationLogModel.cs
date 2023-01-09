using System;
using System.Collections.Generic;
using System.Text;
using B2B_Utils.Model.Common;
using B2B_Utils.Model.OperationsLog;

namespace B2B_Utils.Model
{
    internal class OperationLogModel
    {
        public List<OperationLog> Operation { get; set; }
        private readonly List<string> content;
        public string ERROR_FILE = Environment.CurrentDirectory + "\\AppData\\Error.log";
        public string OUTPUT_DIRECTORY = Environment.CurrentDirectory + "\\AppData\\Output\\";
        public string OUTPUT_LOG = Environment.CurrentDirectory + "\\AppData\\Output.log";

        public OperationLogModel(List<string> content)
        {
            Operation=new List<OperationLog>();
            this.content = content;
        }

        public int LoadLogContent(List<Agencies.IgnoreWord> ignoreList, bool csv, List<Agencies.Agent> agencies = null)
        {
            int count = 0;
            StringBuilder csvContent =new StringBuilder();
            const string csvSeparator = " / ";

            foreach (string con in content) {
                try {
                    string temp;

                    if (String.IsNullOrWhiteSpace(con)) break;
                    if (WorkingWithText.IsContainText(con, ignoreList)) {
                        continue;
                    }
                    if (agencies != null) {
                        if (!WorkingWithText.IsContainText(con, agencies)) continue;
                    }

                    count++;
                    string line = con;
                    OperationLog log = new OperationLog();
                    OperationLogItem item = new OperationLogItem();
                    OperationLogService service = new OperationLogService();
                    OperationLogMetadata metadata = new OperationLogMetadata();

                    log.TimeToLog = WorkingWithText.ExtractString(ref line, ' ');
                    log.LogStatus = WorkingWithText.ExtractString(ref line, ' ');
                    log.JBossMesage = WorkingWithText.ExtractString(ref line, '(', ')');
                    item.TimeStamp = new TimeStamp(WorkingWithText.ExtractString(ref line, "timestamp=", ','));
                    item.Operation = WorkingWithText.ExtractString(ref line, "operation=", ',');
                    item.OperationStatus = WorkingWithText.ExtractString(ref line, "operationStatus=", ',');
                    item.BookingNumber = WorkingWithText.ExtractString(ref line, "bookingNumber=", ',');
                    item.BookingStatus = WorkingWithText.ExtractString(ref line, "bookingStatus=", ',');
                    item.ServerId = WorkingWithText.ExtractString(ref line, "serverId=", ',');
                    item.Tenant = WorkingWithText.ExtractString(ref line, "tenant=", ',');
                    item.Agency = WorkingWithText.ExtractString(ref line, "agency=", ',');
                    item.SalesPoint = WorkingWithText.ExtractString(ref line, "salesPoint=", ',');
                    item.Agent = WorkingWithText.ExtractString(ref line, "agent=", ',');
                    service.ServiceLocator = WorkingWithText.ExtractString(ref line, "serviceLocator=", ',');
                    service.Gds = WorkingWithText.ExtractString(ref line, "gds=", ',');
                    service.ChannelId = WorkingWithText.ExtractString(ref line, "channelId=", ',');
                    service.ServiceType = WorkingWithText.ExtractString(ref line, "serviceType=", ']');
                    item.Services = service;
                    temp = WorkingWithText.ExtractString(ref line, "segments=", ", metadata=");
                    var temparr = temp.Split(new char[] { '[', ']' });
                    List<string> tempList = new List<string>();
                    foreach (string s in temparr) {
                        if (s.Equals("") || s.Equals(" ") || s.Equals(",")) {
                            continue;
                        }
                        tempList.Add(s);
                    }
                    List<ItemSegment> segments = new List<ItemSegment>();
                    for (int i = 0; i < tempList.Count; i++) {
                        if (tempList[i].Length < 35) continue;
                        ItemSegment segment = new ItemSegment();
                        temp = tempList[i];
                        segment.Departure = WorkingWithText.ExtractString(ref temp, "departure=", ',');
                        segment.Arrival = WorkingWithText.ExtractString(ref temp, "arrival=", ',');
                        segment.DepartureDate = WorkingWithText.ExtractString(ref temp, "departureDate=", ',');
                        segment.FlightNumber = WorkingWithText.ExtractString(ref temp, "flightNumber=", ',');
                        segment.Vendor = temp.Substring(temp.IndexOf('=') + 1);
                        segments.Add(segment);
                    }
                    item.Segments = new OperationLogSegments(segments);
                    metadata.ProcessingTime = WorkingWithText.ExtractString(ref line, "processingTime=", ',');
                    try {
                        metadata.InternalErrorMessage = WorkingWithText.ExtractString(ref line, "internalErrorMessage=", ", externalErrorMessage=");
                    } catch (Exception) {
                        try {
                            metadata.InternalErrorMessage = WorkingWithText.ExtractString(ref line, "lastBookingLogErrorMessage=", ", flightType=");
                        } catch {
                            metadata.InternalErrorMessage = "";
                        }
                    }
                    try {
                        metadata.ExternalErrorMessage = WorkingWithText.ExtractString(ref line, "externalErrorMessage=", ", flightType=");
                    } catch (Exception) {
                        metadata.ExternalErrorMessage = "";
                    }
                    metadata.FlightType = WorkingWithText.ExtractString(ref line, "flightType=", ',');
                    metadata.BoardType = WorkingWithText.ExtractString(ref line, "boardType=", ',');
                    metadata.AdultNumber = Int32.Parse(WorkingWithText.ExtractString(ref line, "adultNumber=", ','));
                    metadata.ChildNumber = Int32.Parse(WorkingWithText.ExtractString(ref line, "childNumber=", ','));
                    metadata.InfantNumber = Int32.Parse(WorkingWithText.ExtractString(ref line, "infantNumber=", ','));
                    metadata.YouthNumber = Int32.Parse(WorkingWithText.ExtractString(ref line, "youthNumber=", ','));
                    metadata.SeniourNumber = Int32.Parse(WorkingWithText.ExtractString(ref line, "seniorNumber=", ']'));
                    item.MetaData = metadata;
                    log.Operation = item;
                    Operation.Add(log);
                    if (csv) {
                        csvContent.Append(log.TimeToLog);
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.LogStatus);
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.JBossMesage);
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.Operation.TimeStamp.Time);
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.Operation.TimeStamp.Date);
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.Operation.Operation);
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.Operation.OperationStatus);
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.Operation.BookingNumber);
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.Operation.BookingStatus);
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.Operation.ServerId);
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.Operation.Tenant);
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.Operation.Agency);
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.Operation.SalesPoint);
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.Operation.Agent);
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.Operation.Services.ServiceLocator);
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.Operation.Services.Gds);
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.Operation.Services.ChannelId);
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.Operation.Services.ServiceType);
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.Operation.Segments.Route);
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.Operation.Segments.Dates);
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.Operation.Segments.FlightNumbers);
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.Operation.Segments.Vendors);
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.Operation.MetaData.ProcessingTime);
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.Operation.MetaData.InternalErrorMessage);
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.Operation.MetaData.ExternalErrorMessage);
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.Operation.MetaData.FlightType);
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.Operation.MetaData.BoardType);
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.Operation.MetaData.AdultNumber.ToString());
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.Operation.MetaData.ChildNumber.ToString());
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.Operation.MetaData.InfantNumber.ToString());
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.Operation.MetaData.YouthNumber.ToString());
                        csvContent.Append(csvSeparator);
                        csvContent.Append(log.Operation.MetaData.SeniourNumber.ToString());
                        csvContent.Append(Environment.NewLine);
                    }
                    temp = null;
                    temparr = null;
                    tempList = null;
                } catch (Exception e) {
                    WorkWithFile.WriteFile(ERROR_FILE, "line: " + count.ToString() + " [Source: " + e.Source + "] [Message: " + e.Message + "] [StackTrace: " + e.StackTrace + "]" + Environment.NewLine, true);
                }
            }
            string fileName;
            if (csv) {
                if (string.IsNullOrWhiteSpace(WorkWithFile.ReadFile(OUTPUT_LOG, true)[0])) {
                    fileName = WorkWithFile.GenerateFileName("OL", "csv");
                    WorkWithFile.CreateFile(OUTPUT_DIRECTORY + fileName);
                    WorkWithFile.WriteFile(OUTPUT_LOG, fileName.Substring(0, fileName.IndexOf('.')), true);
                    WorkWithFile.WriteFile(OUTPUT_DIRECTORY + fileName, csvContent.ToString(), true);
                } else {
                    fileName = WorkWithFile.ReadFile(OUTPUT_LOG, true)[0];
                    WorkWithFile.WriteFile(OUTPUT_DIRECTORY + fileName + ".csv", csvContent.ToString(), true);
                }
            }
            return Operation.Count;
        }
    }
}
