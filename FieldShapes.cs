using Orchard;
using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.DisplayManagement.Descriptors;
using Orchard.DisplayManagement.Shapes;
using Orchard.Environment;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.UI.Resources;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// ReSharper disable InconsistentNaming

namespace Raven.Api {
    public class FieldShapes : ApiShapesBase, IShapeTableProvider {
        private readonly Work<WorkContext> _workContext;
        private readonly Work<IResourceManager> _resourceManager;
        private readonly Work<IHttpContextAccessor> _httpContextAccessor;
        private readonly Work<IShapeFactory> _shapeFactory;
        private readonly Work<IContentManager> _contentManager;

        public FieldShapes(
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
        public void Fields_Common_Text(dynamic Display, dynamic Shape, TextWriter Output) {
            using (Display.ViewDataContainer.Model.Node(Shape.ContentField.Name))
            {
                Display.ViewDataContainer.Model.Value = Shape.Value.ToString();
            }
        }

        [Shape(BindingAction.Translate)]
        public void Fields_Input(dynamic Display, dynamic Shape, TextWriter Output) {
            Display.ViewDataContainer.Model.Set(Shape.ContentField.Name, Shape.ContentField.Value);
        }


        [Shape(BindingAction.Translate)]
        public void Fields_Boolean(dynamic Display, dynamic Shape, TextWriter Output) {
            bool? booleanValue = Shape.ContentField.Value;
            Display.ViewDataContainer.Model.Set(Shape.ContentField.Name, booleanValue.HasValue ? booleanValue.Value : booleanValue);
        }

        [Shape(BindingAction.Translate)]
        public void Fields_DateTime(dynamic Display, dynamic Shape, TextWriter Output) {
            Display.ViewDataContainer.Model.Set(Shape.ContentField.Name, Shape.ContentField.DateTime);
        }

        [Shape(BindingAction.Translate)]
        public void Fields_Numeric(dynamic Display, dynamic Shape, TextWriter Output) {
            Display.ViewDataContainer.Model.Set(Shape.ContentField.Name, Shape.ContentField.Value);
        }

        [Shape(BindingAction.Translate)]
        public void Fields_Enumeration(dynamic Display, dynamic Shape, TextWriter Output) {
            string valueToDisplay = string.Empty;
            string[] selectedValues = Shape.ContentField.SelectedValues;
            string[] translatedValues = new string[0];
            if (selectedValues != null) {
                string valueFormat = T("{0}").ToString();
                translatedValues = selectedValues.Select(v => string.Format(valueFormat, T(v).Text)).ToArray();
            }

            Display.ViewDataContainer.Model.Set(Shape.ContentField.Name, translatedValues);
        }

        [Shape(BindingAction.Translate)]
        public void Fields_MediaLibraryPicker(dynamic Display, dynamic Shape, TextWriter Output) {
            var field = Shape.ContentField;
            string name = field.DisplayName;
            var contents = field.MediaParts;
            var mediaShapes = new List<dynamic>();

            foreach (var item in contents) {
                mediaShapes.Add(_contentManager.Value.BuildDisplay(item, "Summary"));
            }
           
            var list = New.List(Name: Shape.ContentField.Name, Items: mediaShapes);
            Display(list);
        }

    }
}
