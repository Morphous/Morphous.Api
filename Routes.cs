using System.Collections.Generic;
using System.Web;
using Orchard.Mvc.Routes;
using System.Linq;
using Orchard.WebApi.Routes;
using System.Web.Http;

namespace Raven.AsyncShapes
{

    public class RavenApiHttpRouteProvider : IHttpRouteProvider
    {

        public RavenApiHttpRouteProvider() {
        }

        public IEnumerable<RouteDescriptor> GetRoutes() {


            return new[] {
                new HttpRouteDescriptor
                {
                    Name = "RavenApiItem",
                    Priority = 100,
                    RouteTemplate = "api/raven.api/{controller}/{id}/{displayType}",
                    Defaults = new { area = "Raven.Api", controller = "item", displayType = "Detail" }
                }
            };

        }


        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }
    }

}