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

namespace Raven.Api.Shapes {
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
        public void Fields_Common_Text(dynamic Display, dynamic Shape) {
            using (Display.ViewDataContainer.Model.Node(Shape.ContentPart))
            {
                using (Display.ViewDataContainer.Model.Node(Shape.ContentField)) {
                    Display.ViewDataContainer.Model.Value = Shape.Value.ToString();
                }
            }
        }

        [Shape(BindingAction.Translate)]
        public void Fields_Common_Text__api__Flat(dynamic Display, dynamic Shape)
        {
            Display.ViewDataContainer.Model.Set(Shape.ContentField.Name, Shape.Value.ToString());
        }

        [Shape(BindingAction.Translate)]
        public void Fields_Input(dynamic Display, dynamic Shape)
        {
            using (Display.ViewDataContainer.Model.Node(Shape.ContentPart))
            {
                using (Display.ViewDataContainer.Model.Node(Shape.ContentField)) {
                    Display.ViewDataContainer.Model.Value = Shape.Value;
                }
            }
        }

        [Shape(BindingAction.Translate)]
        public void Fields_Input__api__Flat(dynamic Display, dynamic Shape) {
            Display.ViewDataContainer.Model.Set(Shape.ContentField.Name, Shape.Value);
        }

        [Shape(BindingAction.Translate)]
        public void Fields_Boolean(dynamic Display, dynamic Shape, TextWriter Output) {
            using (Display.ViewDataContainer.Model.Node(Shape.ContentPart))
            {
                using (Display.ViewDataContainer.Model.Node(Shape.ContentField)) {
                    bool? booleanValue = Shape.ContentField.Value;
                    Display.ViewDataContainer.Model.Value = booleanValue.HasValue ? booleanValue.Value : booleanValue;
                }
            }
        }

        [Shape(BindingAction.Translate)]
        public void Fields_Boolean__api__Flat(dynamic Display, dynamic Shape)
        {
            bool? booleanValue = Shape.ContentField.Value;
            Display.ViewDataContainer.Model.Set(Shape.ContentField.Name, booleanValue.HasValue ? booleanValue.Value : booleanValue);
        }

        [Shape(BindingAction.Translate)]
        public void Fields_DateTime(dynamic Display, dynamic Shape) {
            using (Display.ViewDataContainer.Model.Node(Shape.ContentPart))
            {
                using (Display.ViewDataContainer.Model.Node(Shape.ContentField)) {
                    Display.ViewDataContainer.Model.Value = Shape.ContentField.DateTime;
                }
            }
        }

        [Shape(BindingAction.Translate)]
        public void Fields_DateTime__api__Flat(dynamic Display, dynamic Shape)
        {
            Display.ViewDataContainer.Model.Set(Shape.ContentField.Name, Shape.ContentField.DateTime);
        }

        [Shape(BindingAction.Translate)]
        public void Fields_Numeric(dynamic Display, dynamic Shape) {
            using (Display.ViewDataContainer.Model.Node(Shape.ContentPart))
            {
                using (Display.ViewDataContainer.Model.Node(Shape.ContentField)) {
                    Display.ViewDataContainer.Model.Value = Shape.Value;
                }
            }
        }

        [Shape(BindingAction.Translate)]
        public void Fields_Numeric__api__Flat(dynamic Display, dynamic Shape)
        {
            Display.ViewDataContainer.Model.Set(Shape.ContentField.Name, Shape.Value);
        }

        [Shape(BindingAction.Translate)]
        public void Fields_Enumeration(dynamic Display, dynamic Shape){

            string valueToDisplay = string.Empty;
            string[] selectedValues = Shape.ContentField.SelectedValues;
            string[] translatedValues = new string[0];
            if (selectedValues != null) {
                string valueFormat = T("{0}").ToString();
                translatedValues = selectedValues.Select(v => string.Format(valueFormat, T(v).Text)).ToArray();
            }

            using (Display.ViewDataContainer.Model.Node(Shape.ContentPart))
            {
                using (Display.ViewDataContainer.Model.Node(Shape.ContentField)) {
                    Display.ViewDataContainer.Model.Value = translatedValues;
                }
            }
        }

        [Shape(BindingAction.Translate)]
        public void Fields_Enumeration__api__Flat(dynamic Display, dynamic Shape) {
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
        public void Fields_MediaLibraryPicker(dynamic Display, dynamic Shape)
        {
            using (Display.ViewDataContainer.Model.Node(Shape.ContentPart))
            {
                Fields_MediaLibraryPicker__api__Flat(Display, Shape);
            }
        }

        [Shape(BindingAction.Translate)]
        public void Fields_MediaLibraryPicker__api__Flat(dynamic Display, dynamic Shape) {
            var field = Shape.ContentField;
            string name = field.DisplayName;
            var contents = field.MediaParts;
            var mediaShapes = new List<dynamic>();

            using (Display.ViewDataContainer.Model.List(Shape.ContentField)) {
                foreach (var item in contents) {
                    Display(_contentManager.Value.BuildDisplay(item, "Summary"));
                }
            }
        }

    }
}
