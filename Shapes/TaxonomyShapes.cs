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

using System.Linq;
using Raven.Api.Extensions;
using System.Web.Http.Routing;

namespace Raven.Api.Shapes {
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

        [Shape(bindingType:"Translate")]
        public void Fields_TaxonomyField(dynamic Display, dynamic Shape) {
            using (Display.ViewDataContainer.Model.Node(Shape.ContentPart)) {
                Fields_TaxonomyField__api__Flat(Display, Shape);
            }
        }

        [Shape(bindingType:"Translate")]
        public void Fields_TaxonomyField__api__Flat(dynamic Display, dynamic Shape) {
            System.Web.Mvc.UrlHelper urlHelper = new System.Web.Mvc.UrlHelper(Display.ViewContext.RequestContext);


            using (Display.ViewDataContainer.Model.List(Shape.ContentField)) {
                foreach (var term in (IEnumerable<dynamic>)Shape.Terms) {
                    using (Display.ViewDataContainer.Model.Node("Item")) {
                        Display.ViewDataContainer.Model.Name = term.Name;
                        Display.ViewDataContainer.Model.ResourceUrl =  urlHelper.ItemApiGet((IContent)term);
                    }
                }
            }
        }

        [Shape(bindingType:"Translate")]
        public void Parts_TaxonomyPart(dynamic Display, dynamic Shape) {
            Display(Shape.Taxonomy);
        }

        [Shape(bindingType:"Translate")]
        public void Taxonomy(dynamic Display, dynamic Shape) {
            Display(Shape.Taxonomy);

            using (Display.ViewDataContainer.Model.List("Terms")) {
                DisplayChildren(Display, Shape);
            }
        }

        [Shape(bindingType:"Translate")]
        public void TaxonomyItem(dynamic Display, dynamic Shape) {
            using (Display.ViewDataContainer.Model.Node(Shape.ContentPart)) {

                Shape.Metadata.Alternates.Clear();
                Shape.Metadata.Type = "TaxonomyItemLink";

                System.Web.Mvc.UrlHelper urlHelper = new System.Web.Mvc.UrlHelper(Display.ViewContext.RequestContext);
                Display.ViewDataContainer.Model.Id = Shape.ContentPart.Id;
                Display.ViewDataContainer.Model.Title = Shape.ContentPart.Name;
                Display.ViewDataContainer.Model.DisplayUrl = urlHelper.ItemApiGet((IContent)Shape.ContentPart.ContentItem);

                /* render child elements */
                using (Display.ViewDataContainer.Model.List("Terms")) {
                    if (((IEnumerable<dynamic>)Shape.Items).Any()) {
                        DisplayChildren(Display, Shape);
                    }
                }
            }
        }

        [Shape(bindingType:"Translate")]
        public void Parts_TermPart(dynamic Display, dynamic Shape) {
            //this is to ensure the correct wrapping
            Shape.ContentItems.ContentPart = Shape.ContentPart;

            Display(Shape.ContentItems);
            Display(Shape.Pager);
        }

        
    }
}
