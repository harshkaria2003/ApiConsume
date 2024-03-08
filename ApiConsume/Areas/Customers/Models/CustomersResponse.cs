using ApiConsume.Areas.Airlines.Models;

namespace ApiConsume.Areas.Customers.Models
{
    public class CustomersResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public CustomersModel data { get; set; }
    }
}
