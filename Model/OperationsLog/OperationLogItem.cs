namespace B2B_Utils.Model
{
    public partial class OperationLogItem
    {
        public TimeStamp TimeStamp { get; set; }
        //operation=CANCEL_RESERVATION
        public string Operation { get; set; }
        //operationStatus=FAILED
        public string OperationStatus { get; set; }
        //bookingNumber=05833237
        public string BookingNumber { get; set; }
        //bookingStatus=CONFIRMED
        public string BookingStatus { get; set; }
        //serverId=null
        public string ServerId { get; set; }
        //tenant=BERlogic
        public string Tenant { get; set; }
        //agency=00001003
        public string Agency { get; set; }
        //salesPoint=tutu.ru
        public string SalesPoint { get; set; }
        //agent=berlogic_production@tutu.ru
        public string Agent { get; set; }
        //services=[WorkflowOperationServiceLogItem[]]
        public OperationLogService Services { get; set; }
        //segments=[WorkflowOperationLogItemSegment []]
        public OperationLogSegments Segments { get; set; }
        //metadata=
        public OperationLogMetadata MetaData { get; set; }
    }
}
