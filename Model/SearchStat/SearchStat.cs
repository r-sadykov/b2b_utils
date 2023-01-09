using System.Collections.Generic;

namespace B2B_Utils.Model
{
    public class SearchStat
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string Gmt { get; set; }
        public string Server { get; set; }
        public string ServerId { get; set; }
        public string Tenant { get; set; }
        public string AgencyNumber { get; set; }
        public string SP { get; set; }
        public string Agent { get; set; }
        public string Routes { get; set; }
        public string Pax { get; set; }
        public string Class { get; set; }
        public string TotalFlightsCount { get; set; }
        public string TotalFlightsAfterFilter { get; set; }
        public string TotalRespTime { get; set; }
        public string TotalPreProcessingTime { get; set; }
        public string TotalSearchTime { get; set; }
        public string TotalPostProcessingTime { get; set; }
        public List<SearchStatGds> GdsList { get; set; }
        public SearchStat()
        {
            GdsList = new List<SearchStatGds>();
        }
     }
}
