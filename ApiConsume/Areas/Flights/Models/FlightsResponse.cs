
using ApiConsume.Models;

namespace ApiConsume.Areas.Flights.Models
{
	public class FlightsResponse
	{
		public bool status { get; set; }
		public string message { get; set; }
		public FlightsModel data { get; set; }
	}
}
