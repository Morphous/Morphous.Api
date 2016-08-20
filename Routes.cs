using System.Collections.Generic;
using System.Web;
using Orchard.Mvc.Routes;
using System.Linq;
using Orchard.WebApi.Routes;
using System.Web.Http;

namespace Morphous.AsyncShapes {

    public class MorphousApiHttpRouteProvider : IHttpRouteProvider {

        public MorphousApiHttpRouteProvider() {
        }

        public IEnumerable<RouteDescriptor> GetRoutes() {
            return new[] {
                new HttpRouteDescriptor {
                    Name = "MorphousApiItem",
                    Priority = 100,
                    RouteTemplate = "api/Contents/Item/{id}",
                    Defaults = new { area = "Morphous.Api", controller = "Item" }
                }
            };
        }

        public void GetRoutes(ICollection<RouteDescriptor> routes) {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }
    }
}