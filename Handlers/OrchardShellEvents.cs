//adapted from https://github.com/OnefoursevenLabs/Orchard-Swagger/blob/master/Handlers/OrchardShellEvents.cs

using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Orchard.Environment;
using Orchard.Mvc.Routes;
using Orchard.WebApi.Routes;

namespace Morphous.Api.Handlers
{
    public class OrchardShellEvents : IOrchardShellEvents
    {
        private readonly IEnumerable<IHttpRouteProvider> _httpRouteProviders;

        public OrchardShellEvents(IEnumerable<IHttpRouteProvider> httpRouteProviders) {
            _httpRouteProviders = httpRouteProviders;
        }

        public void Activated() {
            ReloadApiExlorer();
        }

        public void Terminating() {
            GlobalConfiguration.Configuration.Services.Replace(typeof(IApiExplorer),
                new ApiExplorer(new HttpConfiguration()));
        }

        private void ReloadApiExlorer() {
            var config = new HttpConfiguration();
            var allRoutes = new List<RouteDescriptor>();
            allRoutes.AddRange(_httpRouteProviders.SelectMany(provider => provider.GetRoutes()));

            foreach (var routeDescriptor in allRoutes) {
                // extract the WebApi route implementation
                var httpRouteDescriptor = routeDescriptor as HttpRouteDescriptor;

                if (httpRouteDescriptor != null) {
                    if (string.IsNullOrEmpty(httpRouteDescriptor.Name)) {
                        // todo: decide what to do with this guy
                        var area = httpRouteDescriptor.RouteTemplate.Substring(4,
                            httpRouteDescriptor.RouteTemplate.Length - 18 - 4);
                        httpRouteDescriptor.Name = $"Default.{area}";
                    }
                    else {
                        config.Routes.MapHttpRoute(httpRouteDescriptor.Name,
                            httpRouteDescriptor.RouteTemplate,
                            httpRouteDescriptor.Defaults, httpRouteDescriptor.Constraints);
                    }
                }
            }

            GlobalConfiguration.Configuration.Services.Replace(typeof(IApiExplorer), new ApiExplorer(config));
        }
    }
}