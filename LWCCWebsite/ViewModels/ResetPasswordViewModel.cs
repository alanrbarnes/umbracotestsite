using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace LWCCWebsite.ViewModels
{
    public class ResetPasswordViewModel
    {

        [UIHint("Password")]
        [DisplayName("Password")]
        [Required(ErrorMessage = "Please enter your new password")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string Password { get; set; }

        [UIHint("Confirm Password")]
        [DisplayName("Confirm Password")]
        [Required(ErrorMessage = "Please confirm your new password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
