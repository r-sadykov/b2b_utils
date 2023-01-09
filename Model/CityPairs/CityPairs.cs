using System.Collections.Generic;

namespace B2B_Utils.Model
{
    public class CityPairs
    {
        public CityPairs() { Cities = new List<CityPair>(); }

        public List<CityPair> Cities { get; set; }
        public long Count() => Cities.Count;
    }
}
