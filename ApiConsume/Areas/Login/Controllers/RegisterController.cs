using ApiConsume.Areas.Login.DAL;
using ApiConsume.Areas.Login.Models;
using ApiConsume.BAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiConsume.Areas.Login.Controllers
{
   
    [Area("Login")]
    [Route("Login/[Controller]/[Action]")]
    public class RegisterController : Controller
    {
        Uri baseuri = new Uri("http://localhost:5231/api");
        DAL_Helper dal = new DAL_Helper();
        private readonly HttpClient _Client;
        public RegisterController()
        {
            _Client = new HttpClient();
            _Client.BaseAddress = baseuri;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(RegisterModel registraion)
        {
            try
            {
                MultipartFormDataContent fromdata = new MultipartFormDataContent();
                fromdata.Add(new StringContent(registraion.FirstName), "FirstName");
                fromdata.Add(new StringContent(registraion.LastName), "LastName");
                fromdata.Add(new StringContent(registraion.UserName), "UserName");
                fromdata.Add(new StringContent(registraion.Password), "Password");
                fromdata.Add(new StringContent(registraion.Email), "Email");
                fromdata.Add(new StringContent(Convert.ToString(registraion.ISActive)), "ISActive");


                HttpResponseMessage response = await _Client.PostAsync($"{_Client.BaseAddress}/Register/Post", fromdata);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Person Inserted";
                    return RedirectToAction("Login");
                }
            }


            catch (Exception ex)
            {
                TempData["Error"] = "An Error Occured" + ex.Message;
            }
            return RedirectToAction("Login");
        }


        //code for login

        public IActionResult Login()
        {

            return View();
        }


        //login check method
        public async Task<IActionResult> LoginCheck(LoginModel model)
        {
            if (string.IsNullOrEmpty(model.UserName))
            {
                ModelState.AddModelError("UserName", "Enter Username");
            }
            if (string.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError("Password", "Enter Password");
            }
            string apiUrl = $"Login/GetById/{model.UserName}/{model.Password}";
            Console.WriteLine(apiUrl);
            if (ModelState.IsValid)
            {
                var apiResponse = await dal.SendRequestAsync<RegisterModel>(apiUrl, HttpMethod.Get);


                if (apiResponse.IsSuccess)
                {
                    RegisterModel user = apiResponse.Data;  
                    Console.WriteLine(user.ISActive);
                    HttpContext.Session.SetInt32("UserID", user.UserID);
                    HttpContext.Session.SetString("FirstName", user.FirstName);
                    HttpContext.Session.SetString("LastName", user.LastName);
                    HttpContext.Session.SetString("UserName", user.UserName);
                    HttpContext.Session.SetString("Password", user.Password);
                    HttpContext.Session.SetString("Email", user.Email);

                    HttpContext.Session.SetInt32("ISActive", Convert.ToInt32(user.ISActive));

                    if (HttpContext.Session.GetInt32("UserID") != null && HttpContext.Session.GetString("UserName") != null && HttpContext.Session.GetInt32("ISActive") != null)
                    {
                        if (HttpContext.Session.GetInt32("ISActive") == 1)
                        {
                            return RedirectToAction("GET", "DashBoard", new { area = "DashBoard" });

                        }
                        else
                        {
                            return RedirectToAction("Dashboard", "Dashboard", new { area = "Website" });

                        }
                    }
                    else
                    {
                        @ViewData["ErrorMessage"] = "Invalid Username or Password";
                      


                    }

                    /* return RedirectToAction("GET", "Dashboard", new {area ="Admin"});*/

                }
                else
                {
                    @ViewData["ErrorMessage"] = "Invalid Username or Password";
                }
            }

            return View("Login", model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("GET", "DashBoard", new { area = "DashBoard" });
        }

    }
}
