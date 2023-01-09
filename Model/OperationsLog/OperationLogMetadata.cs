namespace B2B_Utils.Model
{
    public partial class OperationLogMetadata
    {
        //processingTime=4376
        public string ProcessingTime { get; set; }
        //internalErrorMessage=ru Cancellation of the ticket is impossible
        public string InternalErrorMessage { get; set; }
        //externalErrorMessage=Аннулирование билета невозможно
        public string ExternalErrorMessage { get; set; }
        //flightType=OW
        public string FlightType { get; set; }
        //boardType=null
        public string BoardType { get; set; }
        //adultNumber=1
        public int AdultNumber { get; set; }
        //childNumber=0
        public int ChildNumber { get; set; }
        //infantNumber=1
        public int InfantNumber { get; set; }
        //youthNumber=0
        public int YouthNumber { get; set; }
        //seniorNumber=0
        public int SeniourNumber { get; set; }
    }
}
