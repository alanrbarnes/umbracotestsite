using Microsoft.AspNetCore.Mvc;
using UdemyTestSite.Helpers;
using UdemyTestSite.ViewModels;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Media.EmbedProviders;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace UdemyTestSite.Controllers
{
    public class AccountController : SurfaceController
    {
        public const string PARTIAL_VIEW_FOLDER = "~/Views/Partials/Login/";
        private readonly IMemberManager _memberManager;
        private readonly IMemberService _memberService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IUmbracoContextAccessor umbracoContextAccessor, 
                                 IUmbracoDatabaseFactory databaseFactory, 
                                 ServiceContext services, 
                                 AppCaches appCaches, 
                                 IProfilingLogger profilingLogger, 
                                 IPublishedUrlProvider publishedUrlProvider,
                                 IMemberManager memberManager, 
                                 IMemberService memberService, 
                                 ILogger<AccountController> logger) 
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _memberManager = memberManager;
            _memberService = memberService;
            _logger = logger;
        }

        public async Task<IActionResult> RenderMyAccount()
        {
            var vm = new AccountViewModel();

            // Are we logged in?
            var currentMember = await _memberManager.GetCurrentMemberAsync();
            if (currentMember == null)
            {
                ModelState.AddModelError("Error", "You must be logged in to view this page");
                return CurrentUmbracoPage();
            }

            // Get member's details
            var member = _memberService.GetByKey(currentMember.Key);
            if (member == null)
            {
                ModelState.AddModelError("Error", "An error occurred retrieving your details");
                return CurrentUmbracoPage();
            }


            // Populate the view model
            vm.Name = member.Name;
            vm.Email = member.Email;
            vm.Username = member.Username;

            return PartialView($"{PARTIAL_VIEW_FOLDER}MyAccount.cshtml", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HandleUpdateDetails(AccountViewModel vm)
        {
            //Is the model valid
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Error", "There has been a problem");
                return CurrentUmbracoPage();
            }

            // Is there a member
            var currentMember = await _memberManager.GetCurrentMemberAsync();
            if (currentMember == null)
            {
                ModelState.AddModelError("Error", "You must be logged in to view this page");
                return CurrentUmbracoPage();
            }

            // Get the current member
            var member = _memberService.GetByKey(currentMember.Key);
            if (member == null)
            {
                ModelState.AddModelError("Error", "You are not logged on.");
                return CurrentUmbracoPage();
            }

            // Update the member
            member.Name = vm.Name;
            member.Email = vm.Email;
            _memberService.Save(member);

            TempData["status"] = "Updated Details";
            return RedirectToCurrentUmbracoPage();
        }

        public async Task<IActionResult> HandlePasswordChange(AccountViewModel vm)
        {
            //Is the model valid
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Error", "There has been a problem with the form");
                return CurrentUmbracoPage();
            }

            // Is there a logged-in member?
            var currentUser = await _memberManager.GetCurrentMemberAsync();
            if (currentUser == null)
            {
                ModelState.AddModelError("Error", "You must be logged in to view this page");
                return CurrentUmbracoPage();
            }

            //Update the Password
            // Update the password
            try
            {
                var changePasswordResult = await _memberManager.ChangePasswordAsync(currentUser, vm.OldPassword, vm.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError("Error", error.Description);
                    }
                    return CurrentUmbracoPage();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error updating the password");
                ModelState.AddModelError("Error", "There was an error updating your password: " + ex.Message);
                return CurrentUmbracoPage();
            }
            //try
            //{
            //    Services.MemberService.SavePassword(member, vm.Password);
            //}
            //catch (System.Exception ex)
            //{
            //    ModelState.AddModelError("Error", "There was an error updating your password" + ex.Message);
            //    return CurrentUmbracoPage();
            //}

            //Return confirmation message to user
            TempData["status"] = "Updated Password";
            return RedirectToCurrentUmbracoPage();  //refresh the page to stop resubmission of form
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
