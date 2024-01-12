using System.ComponentModel.DataAnnotations;

namespace Simple_FrontEnd.Models.Account
{
    public class Login
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}