using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace LWCCWebsite.Controllers
{
    [Route("custom-page")]
    public class CustomViewController : RenderController
    {
        public CustomViewController(ILogger<RenderController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor) : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
        }
        public IActionResult Index()
        {
            // Custom logic, e.g., fetching additional data
            //var model = new CustomPageModel(CurrentPage);

            return View("Hellow World");
        }
    }
}
