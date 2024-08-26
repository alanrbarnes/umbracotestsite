using LWCCWebsite.Interfaces;
using LWCCWebsite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
//using System.Net.Mail;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net.Http;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common;
using Umbraco.Cms.Web.Website.Controllers;
using Umbraco.Extensions;

namespace LWCCWebsite.Controllers
{
    public class ContactController : SurfaceController
    {
        private readonly UmbracoHelper _umbracoHelper;
        private readonly ILogger<ContactController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;


        public ContactController(IUmbracoContextAccessor umbracoContextAccessor,
                                 IUmbracoDatabaseFactory databaseFactory,
                                 ServiceContext services,
                                 AppCaches appCaches,
                                 IProfilingLogger profilingLogger,
                                 IPublishedUrlProvider publishedUrlProvider,
                                 UmbracoHelper umbracoHelper,
                                 ILogger<ContactController> logger,
                                 IConfiguration configuration,
                                 IEmailService emailService)
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _umbracoHelper = umbracoHelper;
            _logger = logger;
            _configuration = configuration;
            _emailService = emailService;
        }

        /// /umbraco/api/MyApi/SayHello?name=John
        [HttpGet]
        public string SayHello(string name)
        {
            //_logger.LogInformation("We are saying hello to {Name}", name);
            return $"Hello {name}";
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult RenderContactForm()
        {
            var vm = new ContactFormViewModel();
            /*
            var siteSettings = _umbracoHelper.ContentAtRoot().DescendantsOrSelfOfType("siteSettings").FirstOrDefault();
            if (siteSettings != null)
            {
                var siteKey = siteSettings.Value<string>("recaptchaSiteKey");
                vm.RecaptchaSiteKey = siteKey;
            }*/
            return PartialView("~/Views/Partials/ContactForm.cshtml", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult HandleContactForm(ContactFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Error", "There was an error with the form");
                return CurrentUmbracoPage();
            }
            //handle recaptcha
            //add recaptcha validation...


            try
            {
                //Create a new contact form in umbraco
                //var newContact = Services.ContentService.Create(vm.Name, CurrentPage.Id, "contactForm");
                var contactForms = _umbracoHelper.ContentAtRoot().DescendantsOrSelfOfType("contactForms").FirstOrDefault();
                if (contactForms != null)
                {
                    var newContact = Services.ContentService.Create("Contact", contactForms.Id, "contactFormEntry");
                    newContact.SetValue("contactName", vm.Name);
                    newContact.SetValue("contactEmail", vm.EmailAddress);
                    newContact.SetValue("contactSubject", vm.Subject);
                    newContact.SetValue("contactComments", vm.Comment);
                    Services.ContentService.SaveAndPublish(newContact);
                }

                //Send out an email to site admin
                _emailService.SendContactNotificationToAdmin(vm);

                //Return confirmation message to user
                TempData["status"] = "OK";
                return RedirectToCurrentUmbracoPage();  //refresh the page to stop resubmission of form
            }
            catch (System.Exception ex)
            {
                //_logger.Error<ContactController>(ex, "Error submitting contact form");
                _logger.LogError(ex, "Error submitting contact form");
                ModelState.AddModelError("Error", "Sorry there was a problem noting your details. Would you please try again.");

            }

            return CurrentUmbracoPage();

        }

        private bool IsCaptchaValid(string token, string secretkey)
        {

            //Sending the token to Google API
            HttpClient httpClient = new HttpClient();
            var res = httpClient
                   .GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret={secretkey}&response={token}")
                   .Result;
            if (res.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return false;
            }

            //Get Response
            string JSONres = res.Content.ReadAsStringAsync().Result;
            dynamic JSONdata = JObject.Parse(JSONres);
            if (JSONdata.success != "true")
            {
                return false;
            }
            //was good?
            return true;
        }

        [HttpPost]
        public IActionResult Submit(ContactFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            // Work with form data here

            return RedirectToCurrentUmbracoPage();
        }


    }
}
