using ApiConsume.Models;

namespace ApiConsume.Areas.Airlines.Models
{
    public class AirlinesResponse
    {
         
        public bool status { get; set; }
        public string message { get; set; }
        public AirlinesModel data { get; set; }
    }
}

