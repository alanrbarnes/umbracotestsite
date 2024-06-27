using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace UdemyTestSite.ViewModels
{
    public class AccountViewModel
    {
        //This is the view model for the Account page
        [DisplayName("Full Name")]
        [Required(ErrorMessage = "Please enter your name")]
        public string Name { get; set; }

        [DisplayName("Email Address")]
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }
        public string Username { get; set; }


        [DataType(DataType.Password)]
        [DisplayName("Old Password")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [UIHint("Password")]
        [DisplayName("Password")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [UIHint("Confirm Password")]
        [DisplayName("Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
