namespace B2B_Utils.Model.Database
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("OperationLog")]
    public partial class OperationLog
    {
        public int Id { get; set; }

        [Required]
        [StringLength(12)]
        public string LoggingTime { get; set; }

        [Required]
        [StringLength(25)]
        public string LogStatus { get; set; }

        [Required]
        [StringLength(25)]
        public string JBossMessage { get; set; }

        [Required]
        [StringLength(10)]
        public string Date { get; set; }

        [StringLength(12)]
        public string Time { get; set; }

        [Required]
        [StringLength(50)]
        public string Operation { get; set; }

        [Required]
        [StringLength(25)]
        public string OperationStatus { get; set; }

        [StringLength(10)]
        public string BookingNumber { get; set; }

        [StringLength(25)]
        public string BookingStatus { get; set; }

        [StringLength(50)]
        public string ServerId { get; set; }

        [Required]
        [StringLength(50)]
        public string Tenant { get; set; }

        [Required]
        [StringLength(10)]
        public string Agency { get; set; }

        [Required]
        [StringLength(50)]
        public string SalesPoint { get; set; }

        [Required]
        [StringLength(100)]
        public string Agent { get; set; }

        [Required]
        [StringLength(50)]
        public string ServiceLocator { get; set; }

        [Required]
        [StringLength(50)]
        public string Gds { get; set; }

        public string ChannelId { get; set; }

        [Required]
        [StringLength(50)]
        public string ServiceType { get; set; }

        public string Route { get; set; }

        public string Dates { get; set; }

        [StringLength(100)]
        public string FlightNumbers { get; set; }

        [StringLength(50)]
        public string Vendors { get; set; }

        [StringLength(25)]
        public string ProcessingTime { get; set; }

        public string InternalErrorMessage { get; set; }

        public string ExternalErrorMessage { get; set; }

        [StringLength(2)]
        public string FlightType { get; set; }

        [StringLength(50)]
        public string BoardType { get; set; }

        public int? AdultNumber { get; set; }

        public int? ChildNumber { get; set; }

        public int? InfantNumber { get; set; }

        public int? YouthNumber { get; set; }

        public int? SeniourNumber { get; set; }
    }
}
