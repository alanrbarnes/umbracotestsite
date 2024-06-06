using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UdemyTestSite.ViewModels
{
    //this is the model for the register page
    public class RegisterViewModel
    {
        [DisplayName("First Name")]
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [DisplayName("Username")]
        [Required(ErrorMessage = "Username is required")]
        [MinLength(5, ErrorMessage = "Username must be at least 5 characters")]
        public string Username { get; set; }

        [DisplayName("Email Address")]
        [Required(ErrorMessage = "Email Address is required")]
        public string EmailAddress { get; set; }

        [UIHint("Password")]
        [DisplayName("Password")]
        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string Password { get; set; }

        [UIHint("Confirm Password")]
        [DisplayName("Confirm Password")]
        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

    }
}
