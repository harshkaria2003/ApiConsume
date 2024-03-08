using ApiConsume.Areas.Airlines.Models;
using ApiConsume.Models;

namespace ApiConsume.Areas.Airports.Models
{
    public class AirportsResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public AirportsModel data { get; set; }
    }
}
