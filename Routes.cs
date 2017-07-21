using System.Collections.Generic;
using System.Web;
using Orchard.Mvc.Routes;
using System.Linq;
using Orchard.WebApi.Routes;
using System.Web.Http;
using Orchard.ContentManagement;
using Morphous.Api.Models;

namespace Morphous.AsyncShapes
{

    public class MorphousApiHttpRouteProvider : IHttpRouteProvider
    {
        private readonly IContentManager _contentManager;


        public MorphousApiHttpRouteProvider(IContentManager contentManager) {
            _contentManager = contentManager;
        }

        public IEnumerable<RouteDescriptor> GetRoutes() {
            var routes = new List<RouteDescriptor>();

            routes.Add(
                new HttpRouteDescriptor
                {
                    Name = "MorphousApiItem",
                    Priority = 100,
                    RouteTemplate = "api/Morphous.api/item/{id}/{displayType}",
                    Defaults = new { area = "Morphous.Api", controller = "item", displayType = "Detail" }
                });

            var apiRoutes = _contentManager.Query<ApiRoutePart>().List();
            foreach (var apiRoute in apiRoutes) {
                if (string.IsNullOrEmpty(apiRoute.Name) || string.IsNullOrEmpty(apiRoute.RouteTemplate))
                    continue;

                routes.Add(new HttpRouteDescriptor
                {
                    Name = apiRoute.Name,
                    RouteTemplate = apiRoute.RouteTemplate,
                    Defaults = new { area = "Morphous.Api", controller = "item", displayType = "Detail", id = apiRoute.Id }
                });
            }

            return routes;
        }


        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }
    }

}