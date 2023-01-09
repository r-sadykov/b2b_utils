namespace B2B_Utils.Model
{
    internal class RoundTripElements
    {
        public CityPairs AddRoundTripEntry(CityPairs pairs)
        {
            CityPairs temp = new CityPairs();
            foreach(CityPair city in pairs.Cities)
            {
                temp.Cities.Add(city);
                CityPair tempCity = new CityPair() {
                    Carrier = city.Carrier,
                    Departure = city.Destination,
                    DepartureLand = city.DestinationLand,
                    DestinationLand = city.DepartureLand,
                    Destination = city.Departure,
                    Land = city.Land
                };
                temp.Cities.Add(tempCity);
            }
            return temp;
        }
    }
}
