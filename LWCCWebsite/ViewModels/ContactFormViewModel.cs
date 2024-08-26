﻿using System.ComponentModel.DataAnnotations;

namespace LWCCWebsite.ViewModels
{
    public class ContactFormViewModel
    {
        //public string Name { get; set; }
        //public string Email { get; set; }
        //public string Message { get; set; }

        [Required]
        [MaxLength(80, ErrorMessage = "Please try and limit to 80 characters")]
        public string? Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string? EmailAddress { get; set; }


        [MaxLength(500, ErrorMessage = "Please try and limit to 500 characters")]
        public string? Comment { get; set; }

        [MaxLength(255, ErrorMessage = "Please try and limit to 255 characters")]
        public string? Subject { get; set; }

        //public string RecaptchaSiteKey { get; set; } 

        //public string RecaptchaSecretKey { get; set; }
    }
}

