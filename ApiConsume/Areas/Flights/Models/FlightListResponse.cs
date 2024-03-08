

using ApiConsume.Models;

namespace ApiConsume.Areas.Flights.Models
{
	public class FlightListResponse
	{
		public bool status { get; set; }
		public string message { get; set; }
		public List<FlightsModel> data { get; set; }
	}
}
