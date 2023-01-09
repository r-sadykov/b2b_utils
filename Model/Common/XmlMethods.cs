using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace B2B_Utils.Model
{
    public class XmlMethods
    {
        public void Serialize(CityPairs pairs)
        {
            Departures deps = new Departures(pairs);
            XmlSerializer serializer = new XmlSerializer(typeof(Departures));
            using (TextWriter writer = new StreamWriter(@"D:\citypairs.xml"))
            {
                serializer.Serialize(writer, deps);
            }
        }
    }

    public class Departures
    {
        public List<Departure> deps;

        public Departures(CityPairs pairs)
        {
            deps = new List<Departure>();
            InToList(pairs);
        }

        public Departures() { }
        public void InToList(CityPairs pairs)
        {
            foreach (CityPair pair in pairs.Cities)
            {
                Departure dep = new Departure(pair.Departure, pair.Destination);
                deps.Add(dep);
            }
        }
    }

    public class Departure
    {
        [XmlAttribute(AttributeName ="code")]
        public string DepartureCode { get; set; }
        [XmlElement(ElementName = "arrival")]
        public string DestinationCode { get; set; }
        public Departure(string dep, string dest)
        {
            DepartureCode = dep;
            DestinationCode = dest;
        }

        public Departure() { }
    }
}
