namespace B2B_Utils.Model
{
    public partial class OperationLog
    {
        //time when log was written. 12 symbols. format: ##:##:##,###
        public string TimeToLog { get; set; }
        //status of message in log. Example: "INFO"
        public string LogStatus { get; set; }
        //JBoss message. Example: "EJB default -23"
        public string JBossMesage { get; set; }
        //timestamp (inside WorkflowOperationLogItem). Example: "2017-07-03T00:00:30.225Z[Etc/UTC]"      
        public OperationLogItem Operation { get; set; }
    }
}
