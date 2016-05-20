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

namespace Raven.Api.Shapes {
    public class TagsShapes : ApiShapesBase, IShapeTableProvider {

        public TagsShapes(
 
            ) {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }
        public void Discover(ShapeTableBuilder builder) {

        }

        [Shape(bindingType:"Translate")]
        public void Parts_Tags_ShowTags(dynamic Display, dynamic Shape) {

            using (Display.ViewDataContainer.Model.Node(Shape.ContentPart)) {
                Parts_Tags_ShowTags__api__Flat(Display, Shape);
            }
        }

        [Shape(bindingType:"Translate")]
        public void Parts_Tags_ShowTags__api__Flat(dynamic Display, dynamic Shape)
        {
            var tagsHtml = new List<IHtmlString>();
            UrlHelper urlHelper = new UrlHelper(Display.ViewContext.RequestContext);
            using (Display.ViewDataContainer.Model.List("Tags"))
            {
                foreach (var t in Shape.Tags)
                {

                    using (Display.ViewDataContainer.Model.Node("Tag"))
                    {
                        Display.ViewDataContainer.Model.Name = t.TagName;
                        Display.ViewDataContainer.Model.DisplayUrl = urlHelper.Action("Search", "Home", new { area = "Orchard.Tags", tagName = (string)t.TagName });
                    }
                }
            }
        }

    }
}
