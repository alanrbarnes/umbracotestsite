using Microsoft.AspNetCore.Mvc;
using UdemyTestSite.Interfaces;
using UdemyTestSite.ViewModels;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

//using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Cms.Web.Common.PublishedModels;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Models;

namespace UdemyTestSite.Controllers
{
    //Handles member registration
    public class RegisterController : SurfaceController
    {
        public const string PARTIAL_VIEW_FOLDER = "~/Views/Partials/";
        private IEmailService _emailService;
        private readonly IMemberService _memberService;
        private readonly IMemberManager _memberManager;
        private readonly ILogger<RegisterController> _logger;

        public RegisterController(IUmbracoContextAccessor umbracoContextAccessor, 
                                  IUmbracoDatabaseFactory databaseFactory, 
                                  ServiceContext services, 
                                  AppCaches appCaches, 
                                  IProfilingLogger profilingLogger, 
                                  IPublishedUrlProvider publishedUrlProvider,
                                  IEmailService emailService,
                                  IMemberService memberService,
                                  IMemberManager memberManager,
                                  ILogger<RegisterController> logger
                                  )
                                  : base(umbracoContextAccessor, 
                                        databaseFactory, services, 
                                        appCaches, profilingLogger, publishedUrlProvider)
        {
            _emailService = emailService;
            _memberService = memberService;
            _logger = logger;
            _memberManager = memberManager;
        }

        #region Register Form
        //renders the register form
        /*public IActionResult RenderRegister()
        {
            var vm = new RegisterViewModel();
            return PartialView(PARTIAL_VIEW_FOLDER + "Register.cshtml", vm);
        }*/

        //Handle the registration form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HandleRegister(RegisterViewModel vm)
        {
            
            //If the form is not valid, return the current page
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
                //return BadRequest(ModelState);
            }

            //Check if there is already a member with that email address
            var existingMember =  _memberService.GetByEmail(vm.EmailAddress);
            if (existingMember != null)
            {
                ModelState.AddModelError("Email", "A member with that email address already exists");
                return CurrentUmbracoPage();
            }

            //Check if there is already a member with that username
            existingMember = _memberService.GetByUsername(vm.Username);
            if (existingMember != null)
            {
                ModelState.AddModelError("User Name", "A member with that username already exists. Please choose a different one.");
                return CurrentUmbracoPage();
            }

            //Send the email confirmation email
            //should wrap in try catch block
            try
            {
                //Create the member in Umbraco with the details from the form
                var newMember = _memberService.CreateMember(vm.Username, vm.EmailAddress, vm.FirstName + " " + vm.LastName, "Member");


                //newMember.PasswordQuestion = "";
                newMember.RawPasswordValue = "";

                // Save the member before setting the password
                _memberService.Save(newMember);

                // Retrieve the member as MemberIdentityUser
                var memberIdentityUser = await _memberManager.FindByIdAsync(newMember.Key.ToString());

                // Set the password
                var identityResult = await _memberManager.AddPasswordAsync(memberIdentityUser, vm.Password);
                if (!identityResult.Succeeded)
                {
                    return BadRequest("Failed to set password.");
                }

                // Assign a role - e.g., "Normal User"
                _memberService.AssignRole(newMember.Id, "Normal User");

                //Create the email confirmation token
                var token = Guid.NewGuid().ToString();
                newMember.SetValue("emailVerifyToken", token);
                _memberService.Save(newMember);

                //send the email
                _emailService.SendVerifyEmailAddressNotification(newMember.Email, token);
            }
            catch (Exception ex)
            {
                //Log the error
                _logger.LogError(ex, "Error sending email confirmation email");
                //Add a model error
                ModelState.AddModelError("Email", "An error occurred sending the email confirmation email. Please try again.");
                return CurrentUmbracoPage();
            }
            

            //Thank the user
            //Return confirmation message to user
            TempData["status"] = "OK";

            

            //redirect rather than return the view to avoid resubmission of the form
            return RedirectToCurrentUmbracoPage();
            //return Ok("Member registered successfully.");
            

            /*
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            //save the user to the database
            //send a confirmation email
            //log the user in
            //redirect to the home page

            return RedirectToCurrentUmbracoPage();
            */

            //return CurrentUmbracoPage();
        }
        #endregion

        #region Verification

        //[HttpGet("/verify{token}")]
        public IActionResult RenderEmailVerification(string token)
        {
            //get token (query string)
            //Look for a member with that token
            var member = _memberService.GetMembersByPropertyValue("emailVerifyToken", token).SingleOrDefault();

            if(member != null)
            {
                //If we find one, set them to verified
                var alreadyVerified = member.GetValue<bool>("emailVerified");
                if(alreadyVerified)
                {
                    //If they are already verified, show a message
                    ModelState.AddModelError("Verified", "This email address has already been verified");
                    return Redirect("/login");
                    //return CurrentUmbracoPage();
                }

                //Otherwise, set them to verified
                member.SetValue("emailVerified", true);
                member.SetValue("emailVerifyDate", DateTime.Now);
                _memberService.Save(member);

                //Thank the user
                TempData["status"] = "OK";

                // Redirect to the login page
                //var childPage = CurrentPage.FirstChild();
                //return RedirectToUmbracoPage(childPage.Key);

                return Redirect("/login");

                //return CurrentUmbracoPage();
            }

            //Otherwise, show an error message
            ModelState.AddModelError("Error", "Invalid verification token");
            return PartialView(PARTIAL_VIEW_FOLDER + "EmailVerification.cshtml");
        }



        #endregion


    }
}



