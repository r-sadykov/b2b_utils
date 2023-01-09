using System.Collections.Generic;

namespace B2B_Utils.Model
{
    public partial class OperationLogSegments
    {
        public string Route { get; }
        public string Dates { get; }
        public string FlightNumbers { get; }
        public string Vendors { get; }

        public OperationLogSegments(List<ItemSegment> segments)
        {
            foreach(ItemSegment segment in segments)
            {
                int i = segments.IndexOf(segment);
                if (segments.IndexOf(segment) > 0 & segments.IndexOf(segment) != segments.Count)
                {
                    Route += ";";
                    Dates += ";";
                    FlightNumbers += ";";
                    Vendors += ";";
                }
                Route += segment.Departure + "-" + segment.Arrival;
                Dates += segment.DepartureDate;
                FlightNumbers += segment.FlightNumber;
                Vendors += segment.Vendor;
            }
        }
    }
}
