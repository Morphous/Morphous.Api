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
    public class TaxonomyShapes : ApiShapesBase, IShapeTableProvider {

        private readonly Work<WorkContext> _workContext;
        private readonly Work<IResourceManager> _resourceManager;
        private readonly Work<IHttpContextAccessor> _httpContextAccessor;
        private readonly Work<IShapeFactory> _shapeFactory;
        private readonly Work<IContentManager> _contentManager;

        public TaxonomyShapes(
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
        public void Fields_TaxonomyField(dynamic Display, dynamic Shape) {
            Display.ViewDataContainer.Model.Set(Shape.ContentField.DisplayName, ((IEnumerable<dynamic>)Shape.Terms).Select(t => t.Name));
        }

        [Shape(BindingAction.Translate)]
        public void Parts_TaxonomyPart(dynamic Display, dynamic Shape) {
            Display(Shape.Taxonomy);
        }

        [Shape(BindingAction.Translate)]
        public void Taxonomy(dynamic Display, dynamic Shape) {
            Display(Shape.Taxonomy);

            using (Display.ViewDataContainer.Model.List("Terms")) {
                DisplayChildren(Display, Shape);
            }
        }

        [Shape(BindingAction.Translate)]
        public void TaxonomyItem(dynamic Display, dynamic Shape) {
            using (Display.ViewDataContainer.Model.Node(Shape.ContentPart.Name)) {

                Shape.Metadata.Alternates.Clear();
                Shape.Metadata.Type = "TaxonomyItemLink";

                UrlHelper urlHelper = new UrlHelper(Display.ViewContext.RequestContext);
                Display.ViewDataContainer.Model.Id = Shape.ContentPart.Id;
                Display.ViewDataContainer.Model.Title = Shape.ContentPart.Name;
                Display.ViewDataContainer.Model.DisplayUrl = urlHelper.ItemDisplayUrl((IContent)Shape.ContentPart.ContentItem);

                /* render child elements */
                using (Display.ViewDataContainer.Model.List("Terms")) {
                    if (((IEnumerable<dynamic>)Shape.Items).Any()) {
                        DisplayChildren(Display, Shape);
                    }
                }
            }
        }

        [Shape(BindingAction.Translate)]
        public void Parts_TermPart(dynamic Display, dynamic Shape) {
            Display(Shape.ContentItems);
            Display(Shape.Pager);
        }
    }
}
