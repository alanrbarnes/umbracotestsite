using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using UdemyTestSite.ViewModels;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Common.Security;
using Umbraco.Cms.Web.Website.Controllers;

namespace UdemyTestSite.Controllers
{
    public class LoginController : SurfaceController
    {
        public const string PARTIAL_VIEW_FOLDER = "~/Views/Partials/Login/";
        private readonly IMemberManager _memberManager;
        private readonly IMemberService _memberService;
        private readonly IMemberSignInManager _signInManager;

        public LoginController(IUmbracoContextAccessor umbracoContextAccessor, 
                               IUmbracoDatabaseFactory databaseFactory, 
                               ServiceContext services, 
                               AppCaches appCaches, 
                               IProfilingLogger profilingLogger, 
                               IPublishedUrlProvider publishedUrlProvider,
                               IMemberManager memberManager,
                               IMemberService memberService,
                               IMemberSignInManager signInManager) 
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _memberManager = memberManager;
            _memberService = memberService;
            _signInManager = signInManager;
        }

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

            if(!string.IsNullOrWhiteSpace(vm.RedirectUrl))
            {
                return Redirect(vm.RedirectUrl);
            }

            return RedirectToCurrentUmbracoPage();
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
