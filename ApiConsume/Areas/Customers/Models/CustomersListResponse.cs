using ApiConsume.Areas.Airlines.Models;

namespace ApiConsume.Areas.Customers.Models
{
    public class CustomersListResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<CustomersModel> data { get; set; }
    }

}
