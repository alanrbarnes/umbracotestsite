using System.ComponentModel.DataAnnotations;

namespace UdemyTestSite.ViewModels
{
    public class ForgottenPasswordViewModel
    {

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid Email Address")]
        public string EmailAddress { get; set; }
    }
}
