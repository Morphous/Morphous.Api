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

// ReSharper disable InconsistentNaming

namespace Raven.Api {
    public class DesignerToolsShapes : ApiShapesBase, IShapeTableProvider {

        private readonly Work<WorkContext> _workContext;
        private readonly Work<IResourceManager> _resourceManager;
        private readonly Work<IHttpContextAccessor> _httpContextAccessor;
        private readonly Work<IShapeFactory> _shapeFactory;
        private readonly Work<IContentManager> _contentManager;

        public DesignerToolsShapes(
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
