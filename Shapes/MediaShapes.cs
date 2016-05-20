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
using Raven.Api;

// ReSharper disable InconsistentNaming

namespace Raven.Shapes.Api {
    public class MediaShapes : ApiShapesBase, IShapeTableProvider {

        public MediaShapes(
            ) {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }
        public void Discover(ShapeTableBuilder builder) { 
        }

        [Shape(bindingType:"Translate")]
        public void Media(dynamic Display, dynamic Shape, TextWriter Output) {
            using (Display.ViewDataContainer.Model.Node("Media")) {
                if (Shape.Meta != null) {
                    Display(Shape.Meta);
                }

                Display(Shape.Header);
                Display(Shape.Content);
                if (Shape.Footer != null) {
                    Display(Shape.Footer);
                }
            }
        }

        [Shape(bindingType:"Translate")]
        public void Parts_Image(dynamic Display, dynamic Shape, TextWriter Output) {
            var mediaPart = Shape.ContentPart.ContentItem.MediaPart;

            Display.ViewDataContainer.Model.Url = mediaPart.MediaUrl;
            Display.ViewDataContainer.Model.AlternateText = mediaPart.AlternateText;
            Display.ViewDataContainer.Model.Width = Shape.ContentPart.Width;
            Display.ViewDataContainer.Model.Height = Shape.ContentPart.Height;
        }

        [Shape(bindingType:"Translate")]
        public void Parts_Image_Summary(dynamic Display, dynamic Shape, TextWriter Output) {
            var mediaPart = Shape.ContentPart.ContentItem.MediaPart;

            Display.ViewDataContainer.Model.Url = mediaPart.MediaUrl;
            Display.ViewDataContainer.Model.AlternateText = mediaPart.AlternateText;
            Display.ViewDataContainer.Model.Width = Shape.ContentPart.Width;
            Display.ViewDataContainer.Model.Height = Shape.ContentPart.Height;
        }

    }
}
