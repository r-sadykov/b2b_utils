using System.ComponentModel.DataAnnotations;

namespace B2B_Utils.Model.Database
{
    public partial class OperationLogInDb
    {
        [Key]
        public int Id { get; set; }
        public string LoggingTime { get; set; }
        public string LogStatus { get; set; }
        public string JBossMessage { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Operation { get; set; }
        public string OperationStatus { get; set; }
        public string BookingNumber { get; set; }
        public string BookingStatus { get; set; }
        public string ServerId { get; set; }
        public string Tenant { get; set; }
        public string Agency { get; set; }
        public string SalesPoint { get; set; }
        public string Agent { get; set; }
        public string ServiceLocator { get; set; }
        public string Gds { get; set; }
        public string ChannelId { get; set; }
        public string ServiceType { get; set; }
        public string Route { get; set; }
        public string Dates { get; set; }
        public string FlightNumbers { get; set; }
        public string Vendors { get; set; }
        public string ProcessingTime { get; set; }
        public string InternalErrorMessage { get; set; }
        public string ExternalErrorMessage { get; set; }
        public string FlightType { get; set; }
        public string BoardType { get; set; }
        public int AdultNumber { get; set; }
        public int ChildNumber { get; set; }
        public int InfantNumber { get; set; }
        public int YouthNumber { get; set; }
        public int SeniourNumber { get; set; }
    }
}
