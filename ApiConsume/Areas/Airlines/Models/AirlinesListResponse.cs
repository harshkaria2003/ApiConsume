
using ApiConsume.Models;

namespace ApiConsume.Areas.Airlines.Models
{
    public class AirlinesListResponse
    {
         
        public bool status { get; set; }
        public string message { get; set; }
        public List<AirlinesModel> data { get; set; }
    }
}

