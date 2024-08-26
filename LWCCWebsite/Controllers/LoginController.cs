using LWCCWebsite.Interfaces;
using LWCCWebsite.ViewModels;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.Security;
using Umbraco.Cms.Web.Website.Controllers;

namespace LWCCWebsite.Controllers
{
    public class LoginController : SurfaceController
    {
        public const string PARTIAL_VIEW_FOLDER = "~/Views/Partials/Login/";
        private readonly IMemberManager _memberManager;
        private readonly IMemberService _memberService;
        private readonly IMemberSignInManager _signInManager;
        private readonly IEmailService _emailService;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IUmbracoContextAccessor umbracoContextAccessor,
                               IUmbracoDatabaseFactory databaseFactory,
                               ServiceContext services,
                               AppCaches appCaches,
                               IProfilingLogger profilingLogger,
                               IPublishedUrlProvider publishedUrlProvider,
                               IMemberManager memberManager,
                               IMemberService memberService,
                               IMemberSignInManager signInManager,
                               IEmailService emailService,
                               ILogger<LoginController> logger)
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _memberManager = memberManager;
            _memberService = memberService;
            _signInManager = signInManager;
            _emailService = emailService;
            _logger = logger;
        }

        #region Login
        public IActionResult RenderLogin()
        {
            var vm = new LoginViewModel();
            vm.RedirectUrl = HttpContext.Request.GetEncodedUrl();
            //vm.RedirectUrl = HttpContext.Request.Path;
            return PartialView($"{PARTIAL_VIEW_FOLDER}Login.cshtml", vm);
        }


        //Handle the registration form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HandleLogin(LoginViewModel vm)
        {
            //Check if Model is ok
            //If the form is not valid, return the current page
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
                //return BadRequest(ModelState);
            }

            //Check if the member exists
            var member = _memberService.GetByUsername(vm.Username);
            if (member == null)
            {
                ModelState.AddModelError("Login", "Username or password is incorrect");
                return CurrentUmbracoPage();
            }

            //Check if the member is locked out
            if (member.IsLockedOut)
            {
                ModelState.AddModelError("Login", "Account is locked out");
                return CurrentUmbracoPage();
            }

            //Check if they have validated their email address
            var emailVerified = member.GetValue<bool>("emailVerified");
            if (!emailVerified)
            {
                ModelState.AddModelError("Login", "Email address has not been verified");
                return CurrentUmbracoPage();
            }

            var result = await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, isPersistent: false, lockoutOnFailure: false);

            //if (result.Succeeded)
            //{
            //    // Redirect to the desired page after successful login
            //    return RedirectToAction("Index", "Home");
            //}

            //Check if credentials are valid
            //log them in
            if (!result.Succeeded) //(!_memberManager.Login(vm.Username, vm.Password))
            {
                ModelState.AddModelError("Login", "Username or password is incorrect");
                return CurrentUmbracoPage();
            }

            if (!string.IsNullOrWhiteSpace(vm.RedirectUrl))
            {
                return Redirect(vm.RedirectUrl);
            }

            return RedirectToCurrentUmbracoPage();
        }
        #endregion

        #region Forgotten Password
        public IActionResult RenderForgottenPassword()  //not used
        {
            var vm = new ForgottenPasswordViewModel();
            return PartialView($"{PARTIAL_VIEW_FOLDER}ForgottenPassword.cshtml", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HandleForgottenPassword(ForgottenPasswordViewModel vm)
        {
            //Is the model valid?
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            //Do we even have a member with this email address?
            //If not, error and return the current page
            var member = _memberService.GetByEmail(vm.EmailAddress);
            var memberManager = await _memberManager.FindByEmailAsync(vm.EmailAddress);
            if (memberManager == null)
            {
                ModelState.AddModelError("Error", "No member found with that email address");
                return CurrentUmbracoPage();
            }

            //Create the reset token
            //var resetToken = Guid.NewGuid().ToString();
            var resetToken = await _memberManager.GeneratePasswordResetTokenAsync(memberManager);

            //Set the reset expiry date (now + 12 hours)
            var expiryDate = DateTime.Now.AddHours(12);

            //Save the member
            //member.SetValue("resetExpiryDate", expiryDate);
            //member.SetValue("resetLinkToken", resetToken);
            //_memberService.Save(member);

            // Save the token and expiry date to custom properties if needed
            //var memberUmbraco = _memberService.GetByKey(new Guid(memberManager.Id));
            member.SetValue("resetExpiryDate", expiryDate);
            member.SetValue("resetLinkToken", resetToken);
            _memberService.Save(member);

            //Fire off the email - reset password email
            _emailService.SendResetPasswordNotification(vm.EmailAddress, resetToken);

            _logger.LogInformation($"Password reset email sent to {vm.EmailAddress}");

            //thanks
            TempData["Status"] = "OK";

            return RedirectToCurrentUmbracoPage();

        }

        #endregion


        #region Reset Password
        public IActionResult RenderResetPassword()  //not used
        {
            var vm = new RegisterViewModel();
            return PartialView($"{PARTIAL_VIEW_FOLDER}Register.cshtml", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HandleResetPassword(ResetPasswordViewModel vm)
        {
            //Get the reset token
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            // Get the reset token from the query string
            var token = Request.Query["token"].ToString(); //used QueryString instead of Query

            if (string.IsNullOrWhiteSpace(token))
            {
                _logger.LogWarning("Reset Password - No reset token found");
                ModelState.AddModelError("Error", "Invalid reset token");
                return CurrentUmbracoPage();
            }
            //Get the member with the token
            var member = _memberService.GetMembersByPropertyValue("resetLinkToken", token).SingleOrDefault();
            if (member == null)
            {
                ModelState.AddModelError("Error", "That link is no longer valid");
                return CurrentUmbracoPage();
            }

            // Check if the token has expired
            var membersTokenExpiryDate = member.GetValue<DateTime>("resetExpiryDate");
            var currentTime = DateTime.Now;
            if (currentTime.CompareTo(membersTokenExpiryDate) > 0)
            {
                ModelState.AddModelError("Error", "Sorry the link has expired, please use the Forgotten Password page to generate a new one");
                return CurrentUmbracoPage();
            }


            // Retrieve the member as MemberIdentityUser
            var memberIdentityUser = await _memberManager.FindByIdAsync(member.Key.ToString());

            // Set the password
            var identityResult = await _memberManager.ResetPasswordAsync(memberIdentityUser, token, vm.Password);
            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return CurrentUmbracoPage();
                //return BadRequest("Failed to set password.");
            }

            // Clear the reset token and expiry date, and unlock the member
            member.SetValue("resetLinkToken", null);
            member.SetValue("resetExpiryDate", null);
            member.IsLockedOut = false;
            _memberService.Save(member);

            //send out the email notification that the password changed
            _emailService.SendPasswordChangedNotification(member.Email);

            //Thanks
            TempData["Status"] = "OK";
            _logger.LogInformation($"Password reset for {member.Username}");

            return RedirectToCurrentUmbracoPage();

        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }



    }
}
