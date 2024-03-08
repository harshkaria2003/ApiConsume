
using ApiConsume.Areas.Airlines.Models;
using ApiConsume.Models;

namespace ApiConsume.Areas.Airports.Models
{
    public class AirportListResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<AirportsModel> data { get; set; }
    }
}
