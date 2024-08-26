using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Umbraco.Cms.Web.Common.Controllers;

namespace LWCCWebsite.Controllers
{
    // /umbraco/api/GenericApi/GetProducts
    public class GenericApiController : UmbracoApiController
    {
        public IActionResult GetProducts()
        {
            var products = new List<SimpleProduct>()

            {
                new SimpleProduct(){Id = "abc", Title = "Product title"},
                new SimpleProduct(){Id = "def", Title = "Product title two"}
            };
            return Ok(products);
        }

        private class SimpleProduct
        {
            public string Id { get; set; }
            public string Title { get; set; }
        }

    }
}
