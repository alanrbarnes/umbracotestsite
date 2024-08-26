using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LWCCWebsite.ViewModels
{
    //The view model for the login form
    public class LoginViewModel
    {
        public LoginViewModel()
        {
            RedirectUrl = GetAbsoluteCurrentUrl();
        }

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string RedirectUrl { get; set; }

        private string GetAbsoluteCurrentUrl()
        {
            var httpContext = new HttpContextAccessor().HttpContext;
            string absoluteUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}{httpContext.Request.QueryString}";
            return absoluteUrl;
        }
    }
}
