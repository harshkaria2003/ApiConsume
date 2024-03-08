namespace ApiConsume.Areas.Airports.Models
{
    public class AirportsModel
    {
        public int? airport_id { get; set; }

        public string  airport_code  { get; set; }

        public string airport_name { get; set; }

        public string city { get; set; }

        public string country { get; set; }

        public int  departure_airport_id  { get; set; }

        public int  destination_airport_id  { get; set; }

      


    }
}
