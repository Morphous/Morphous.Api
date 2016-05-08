using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.DisplayManagement;
using Orchard.DisplayManagement.Descriptors;
using Orchard.DisplayManagement.Shapes;
using Orchard.Environment;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.Mvc.Html;
using Orchard.UI.Resources;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Linq;

// ReSharper disable InconsistentNaming

namespace Raven.Api {
    public class TagsShapes : ApiShapesBase, IShapeTableProvider {

        private readonly Work<WorkContext> _workContext;
        private readonly Work<IResourceManager> _resourceManager;
        private readonly Work<IHttpContextAccessor> _httpContextAccessor;
        private readonly Work<IShapeFactory> _shapeFactory;
        private readonly Work<IContentManager> _contentManager;

        public TagsShapes(
            Work<WorkContext> workContext,
            Work<IResourceManager> resourceManager,
            Work<IHttpContextAccessor> httpContextAccessor,
            Work<IShapeFactory> shapeFactory,
            Work<IContentManager> contentManager
            ) {
            _workContext = workContext;
            _resourceManager = resourceManager;
            _httpContextAccessor = httpContextAccessor;
            _shapeFactory = shapeFactory;
            _contentManager = contentManager;

            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }
        public dynamic New { get { return _shapeFactory.Value; } }
        public void Discover(ShapeTableBuilder builder) {

        }

        [Shape(BindingAction.Translate)]
        public void Parts_Tags_ShowTags(dynamic Display, dynamic Shape) {

            var tagsHtml = new List<IHtmlString>();
            UrlHelper urlHelper = new UrlHelper(Display.ViewContext.RequestContext);

            using (Display.ViewDataContainer.Model.List("TagsPart")) {

                foreach (var t in Shape.Tags) {

                    using (Display.ViewDataContainer.Model.Node("Tag")) {
                        Display.ViewDataContainer.Model.Name = t.TagName;
                        Display.ViewDataContainer.Model.DisplayUrl = urlHelper.Action("Search", "Home", new { area = "Orchard.Tags", tagName = (string)t.TagName });
                    }
                }
            }
        }

    }
}
