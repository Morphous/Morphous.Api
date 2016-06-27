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

namespace Morphous.Api.Controllers {
    [CamelCaseController]
    public class ItemController : ApiController {

        private readonly IContentManager _contentManager;
        private readonly IShapeTranslate _serializer;
        private readonly IBindingTypeCreateAlterations _alterations;

        public dynamic Shape { get; set; }
        public IOrchardServices Services { get; private set; }
        public Localizer T { get; set; }

        public ItemController(
            IContentManager contentManager, 
            IShapeTranslate serializer, 
            IShapeFactory shapeFactory, 
            IOrchardServices services,
            IBindingTypeCreateAlterations alterations) {

            _contentManager = contentManager;
            _serializer = serializer;
            Shape = shapeFactory;
            Services = services;
            T = NullLocalizer.Instance;
            _alterations = alterations;

        }

        // GET api/<controller>
        public IEnumerable<string> Get() {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public IHttpActionResult Get(int id, string displayType) {

            var contentItem = _contentManager.Get(id, VersionOptions.Published);

            if (contentItem == null)
                return NotFound();

            if (!Services.Authorizer.Authorize(Permissions.ViewContent, contentItem, T("Cannot view content"))) {
                return new NotFoundWithMessageResult(T("Cannot view content").ToString());
            }

            dynamic model;
            using (_alterations.CreateScope("Translate")) {
                model = _contentManager.BuildDisplay(contentItem, displayType);
            }
                
            var vm = _serializer.Display(model);

            return Ok(vm);

        }


        public class NotFoundWithMessageResult : IHttpActionResult {
            private string message;

            public NotFoundWithMessageResult(string message) {
                this.message = message;
            }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken) {
                var response = new HttpResponseMessage(HttpStatusCode.NotFound);
                response.Content = new StringContent(message);
                return Task.FromResult(response);
            }
        }

    }
}