using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;
using Orchard;

namespace Raven.Api.Services
{
    public interface IAsyncTemplateProvider : IDependency
    {
        string GetTemplateUrl(RequestContext context, string ContentType, string displayType);
    }
}
