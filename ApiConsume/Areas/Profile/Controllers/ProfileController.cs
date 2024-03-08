using ApiConsume.BAL;
using Microsoft.AspNetCore.Mvc;

namespace ApiConsume.Areas.Profile.Controllers
{
    [CheckAccess]
    [Area("Profile")]
    [Route("Profile/{Controller}/{Action}")]
    public class ProfileController : Controller
    {
        public IActionResult Profile()
        {
            string firstName = HttpContext.Session.GetString("FirstName");
            string lastName = HttpContext.Session.GetString("LastName");
            string email = HttpContext.Session.GetString("Email");
            string username = HttpContext.Session.GetString("UserName");
            string password = HttpContext.Session.GetString("password");

            ViewData["firstName"] = firstName;
            ViewData["lastName"] = lastName;
            ViewData["email"] = email;
            ViewData["username"] = username;
            ViewData["password"] = password;

            return View("ProfileList");

        }
    }
}
