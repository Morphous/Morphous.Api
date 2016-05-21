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
    public class LayoutsShapes : ApiShapesBase, IDependency {

        private readonly Work<IBindingTypeDisplayAlterations> _alterations;

        public LayoutsShapes(
                Work<IBindingTypeDisplayAlterations> alterations
            ) {
            T = NullLocalizer.Instance;
            _alterations = alterations;
        }

        public Localizer T { get; set; }

        [Shape(bindingType: "Translate")]
        public void Parts_Layout(dynamic Display, dynamic Shape) {
            using (Display.ViewDataContainer.Model.Node(Shape.ContentPart)) {
                Parts_Layout__api__Flat(Display, Shape);
            }
        }

        [Shape(bindingType:"Translate")]
        public void Parts_Layout__api__Flat(dynamic Display, dynamic Shape) {
            using (_alterations.Value.DisplayScope("Display")) {
                Display.ViewDataContainer.Model.Set("Content",Display(Shape.LayoutRoot).ToString());
            }
        }
    }
}
