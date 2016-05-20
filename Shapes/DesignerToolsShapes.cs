using System.IO;

using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.DisplayManagement.Descriptors;
using Orchard.Environment;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.UI.Resources;
using Orchard;
using Orchard.DisplayManagement.Shapes;
using System;

namespace Raven.Api.Shapes {
    public class DesignerToolsShapes : ApiShapesBase, IShapeTableProvider {

        public DesignerToolsShapes(
            ) {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }
        public void Discover(ShapeTableBuilder builder) {

        }

        [Shape(bindingType:"Translate")]
        public void ShapeTracingWrapper(dynamic Display, dynamic Shape, TextWriter Output) {
            if (Shape.IgnoreShapeTracer == null || !(bool)Shape.IgnoreShapeTracer) {
                using (Display.ViewDataContainer.Model.Node(string.Concat(Shape.ShapeId.ToString(), "-", Shape.Metadata.Type))) {
                    Display.ViewDataContainer.Model.ShapeType = Shape.Metadata.Type;
                    Display.ViewDataContainer.Model.DisplayType = Shape.Metadata.DisplayType;
                    Display.ViewDataContainer.Model.Position = Shape.Metadata.Position;
                    Display.ViewDataContainer.Model.Alternates = Shape.Metadata.Alternates;
                    Display.ViewDataContainer.Model.Wrappers = Shape.Metadata.Wrappers;
                    Display.ViewDataContainer.Model.ShapeId = Shape.ShapeId;

                }
            }

            Display(Shape.Metadata.ChildContent);
        }


     

    }
}
