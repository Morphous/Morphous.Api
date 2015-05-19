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

// ReSharper disable InconsistentNaming

namespace Raven.Api {
    public class MediaShapes : ApiShapesBase, IShapeTableProvider {

        private readonly Work<WorkContext> _workContext;
        private readonly Work<IResourceManager> _resourceManager;
        private readonly Work<IHttpContextAccessor> _httpContextAccessor;
        private readonly Work<IShapeFactory> _shapeFactory;
        private readonly Work<IContentManager> _contentManager;

        public MediaShapes(
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


        [Shape(BindingAction.Translate)]
        public void Parts_Image(dynamic Display, dynamic Shape, TextWriter Output) {
            var mediaPart = Shape.ContentPart.ContentItem.MediaPart;

            Display.ViewDataContainer.Model.Url = mediaPart.MediaUrl;
            Display.ViewDataContainer.Model.AlternateText = mediaPart.AlternateText;
            Display.ViewDataContainer.Model.Width = Shape.ContentPart.Width;
            Display.ViewDataContainer.Model.Height = Shape.ContentPart.Height;
        }

        [Shape(BindingAction.Translate)]
        public void Parts_Image_Summary(dynamic Display, dynamic Shape, TextWriter Output) {
            var mediaPart = Shape.ContentPart.ContentItem.MediaPart;

            Display.ViewDataContainer.Model.Url = mediaPart.MediaUrl;
            Display.ViewDataContainer.Model.AlternateText = mediaPart.AlternateText;
            Display.ViewDataContainer.Model.Width = Shape.ContentPart.Width;
            Display.ViewDataContainer.Model.Height = Shape.ContentPart.Height;
        }

    }
}
