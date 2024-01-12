using System.ComponentModel.DataAnnotations;

namespace Simple_FrontEnd.Models.Account
{
    public class EditUser
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }


        [MinLength(6, ErrorMessage = "The Password field must be a minimum of 6 characters")]
        public string Password { get; set; }

        public EditUser() { }

        public EditUser(Users user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
        }
    }
}