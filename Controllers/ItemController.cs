using Orchard;
using Orchard.ContentManagement;
using Orchard.Core.Contents;
using Orchard.DisplayManagement;
using Orchard.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Morphous.Api.DisplayManagement;
using Morphous.Api.Filters;

namespace Morphous.Api.Controllers {
    [CamelCaseController]
    [NotifyFilter]
    public class ItemController : ApiController {
        private readonly IContentManager _contentManager;
        private readonly IShapeTranslate _serializer;
        private readonly IBindingTypeCreateAlterations _alterations;
        
        public IOrchardServices Services { get; private set; }
        public Localizer T { get; set; }

        public ItemController(
            IContentManager contentManager, 
            IShapeTranslate serializer,
            IOrchardServices services,
            IBindingTypeCreateAlterations alterations) {
            
            _contentManager = contentManager;
            _serializer = serializer;
            Services = services;
            T = NullLocalizer.Instance;
            _alterations = alterations;
        }

        // /api/Contents/Item/72
        [HttpGet]
        public IHttpActionResult Display(int id, string displayType = "Detail") {
            var contentItem = _contentManager.Get(id, VersionOptions.Published);

            if (contentItem == null)
                return NotFound();

            if (!Services.Authorizer.Authorize(Permissions.ViewContent, contentItem, T("Cannot view content"))) {
                return Unauthorized();
            }

            dynamic model;
            using (_alterations.CreateScope("Translate")) {
                model = _contentManager.BuildDisplay(contentItem, displayType);
            }

            var vm = _serializer.Display(model);

            return Ok(vm);
        }
        
        // /api/Contents/Item/72?version=5
        [HttpGet]
        public IHttpActionResult Preview(int id, int version, string displayType = "Detail") {
            var versionOptions = VersionOptions.Number((int)version);
            
            var contentItem = _contentManager.Get(id, versionOptions);
            if (contentItem == null)
                return NotFound();

            if (!Services.Authorizer.Authorize(Permissions.PreviewContent, contentItem, T("Cannot preview content"))) {
                return Unauthorized();
            }

            dynamic model;
            using (_alterations.CreateScope("Translate")) {
                model = _contentManager.BuildDisplay(contentItem, displayType);
            }

            var vm = _serializer.Display(model);

            return Ok(vm);
        }
    }
}