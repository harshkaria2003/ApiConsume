namespace ApiConsume.Areas.Flights.Models
{
    public class FlightsModel
    {
        public int?  flight_id  { get; set; }

        public int?   airline_id { get; set; }

        public string flight_number  { get; set; }

        public DateTime departure_datetime { get; set; }

        public DateTime arrival_datetime { get; set; }

        public int available_seats { get; set; }

        public int? airport_id { get; set; }


    }
}
