using Orchard.DisplayManagement.Implementation;
using Orchard.Environment.Extensions;
using Orchard.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Raven.Api.Services
{
    public class AcceptHeaderAlternatesFactory : ShapeDisplayEvents
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Lazy<List<string>> _acceptAlternates;

        public AcceptHeaderAlternatesFactory(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            _acceptAlternates = new Lazy<List<string>>(() => {

                var httpContext = _httpContextAccessor.Current();

                if (httpContext == null)
                {
                    return null;
                }
                
                var request = httpContext.Request;

                if (request == null)
                {
                    return null;
                }

                var acceptsHeader = request.Headers["Accept-Alternates"];

                if (string.IsNullOrEmpty(acceptsHeader)) {
                    return null;
                }

                return acceptsHeader.Split(new char[] { ',', ';' }).ToList();
            });
        }

        public override void Displaying(ShapeDisplayingContext context)
        {

            context.ShapeMetadata.OnDisplaying(displayedContext => {

                if (_acceptAlternates.Value == null || !_acceptAlternates.Value.Any())
                {
                    return;
                }

                // prevent applying alternate again, c.f. http://orchard.codeplex.com/workitem/18298
                if (displayedContext.ShapeMetadata.Alternates.Any(x => x.Contains("__api__")))
                {
                    return;
                }

                // appends alternates to current ones
                displayedContext.ShapeMetadata.Alternates = displayedContext.ShapeMetadata.Alternates.SelectMany(
                    alternate => new[] { alternate }.Union(_acceptAlternates.Value.Select(a => alternate + "__api__" + a))
                    ).ToList();

                 // appends [ShapeType]__api__[alternate] alternates
                displayedContext.ShapeMetadata.Alternates = _acceptAlternates.Value.Select(a => displayedContext.ShapeMetadata.Type + "__api__" + a)
                    .Union(displayedContext.ShapeMetadata.Alternates)
                    .ToList();
            });

        }
    }
}