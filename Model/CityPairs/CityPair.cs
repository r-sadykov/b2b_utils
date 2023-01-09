using System.Xml.Serialization;

namespace B2B_Utils.Model
{
    public class CityPair
    {
        [XmlIgnore]
        public string Carrier { get; set; }
        [XmlAttribute("code")]
        public string Departure { get; set; }
        [XmlElement("arrival")]
        public string Destination { get; set; }
        [XmlIgnore]
        public string DepartureLand { get; set; }
        [XmlIgnore]
        public string DestinationLand { get; set; }
        [XmlIgnore]
        public string Land { get; set; }
    }
}
