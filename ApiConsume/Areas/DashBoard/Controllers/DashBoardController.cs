
using ApiConsume.Areas.DashBoard.Models;
using ApiConsume.BAL;
using ApiConsume.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ApiConsume.Areas.DashBoard.Controllers
{
    [CheckAccess]
    [Area("DashBoard")]
    [Route("DashBoard/{Controller}/{Action}")]
    public class DashBoardController : Controller
    {
        string baseAddress ="http://localhost:5231/api/";
        private readonly HttpClient _client;

        /*public DashBoardController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAddress;
        }*/
        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> GET()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.GetAsync("DashBoard");
                if (responseMessage.IsSuccessStatusCode)
                {
                    string data = responseMessage.Content.ReadAsStringAsync().Result;
					DashBoardModel jsonObject = JsonConvert.DeserializeObject<DashBoardModel>(data);
                    var dataOfObject = jsonObject;
                    return View("DEshboardLists", dataOfObject);
                }
                else
                {
                    Console.WriteLine("Error in api");
                }
                return View("DEshboardLists");
            }
        }




        /*public IActionResult GET()
        {
            List<DashBoardModel> dash = new List<DashBoardModel>();
            HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}DashBoard").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                dynamic jsonobject = JsonConvert.DeserializeObject(data);

                var dataofObject = jsonobject.data;
                var extractedDataJson = JsonConvert.SerializeObject(dataofObject, Formatting.Indented);
                dash = JsonConvert.DeserializeObject<List<DashBoardModel>>(extractedDataJson);
            }
            
            return View("DEshboardLists", dash);
        }*/
    }
}

