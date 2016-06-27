using Orchard.ContentManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Morphous.Api.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string ItemApiGet(this UrlHelper urlHelper, IContent content, string displayType = "Detail") {
            return urlHelper.RouteUrl("MorphousApiItem", new { httpRoute = true, id = content.Id, displayType = displayType });
        }
    }
}