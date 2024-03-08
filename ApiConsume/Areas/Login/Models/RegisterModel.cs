using System.ComponentModel.DataAnnotations;

namespace ApiConsume.Areas.Login.Models
{
    public class RegisterModel
    {
        public int UserID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public Boolean ISActive { get; set; }
    }
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}


