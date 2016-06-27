using Orchard;
using Orchard.ContentManagement;
using Orchard.DisplayManagement;
using Orchard.DisplayManagement.Descriptors;
using Orchard.DisplayManagement.Shapes;
using Orchard.Environment;
using Orchard.Fields.Settings;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.UI.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Morphous.Api.Shapes {
    public class FieldShapes : ApiShapesBase, IShapeTableProvider {
        private readonly Work<IContentManager> _contentManager;
        private readonly Work<IBindingTypeCreateAlterations> _alterations;

        public FieldShapes(
            Work<IContentManager> contentManager,
            Work<IBindingTypeCreateAlterations> alterations
            ) {
            _contentManager = contentManager;
            _alterations = alterations;
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }
        public void Discover(ShapeTableBuilder builder) {

        }

        [Shape(bindingType:"Translate")]
        public void Fields_Common_Text(dynamic Display, dynamic Shape) {
            using (Display.ViewDataContainer.Model.Node(Shape.ContentPart))
            {
                using (Display.ViewDataContainer.Model.Node(Shape.ContentField)) {
                    Display.ViewDataContainer.Model.Value = Shape.Value.ToString();
                }
            }
        }

        [Shape(bindingType:"Translate")]
        public void Fields_Common_Text__api__Flat(dynamic Display, dynamic Shape)
        {
            Display.ViewDataContainer.Model.Set(Shape.ContentField.Name, Shape.Value.ToString());
        }

        [Shape(bindingType:"Translate")]
        public void Fields_Input(dynamic Display, dynamic Shape)
        {
            using (Display.ViewDataContainer.Model.Node(Shape.ContentPart))
            {
                using (Display.ViewDataContainer.Model.Node(Shape.ContentField)) {
                    Display.ViewDataContainer.Model.Value = Shape.Value;
                }
            }
        }

        [Shape(bindingType:"Translate")]
        public void Fields_Input__api__Flat(dynamic Display, dynamic Shape) {
            Display.ViewDataContainer.Model.Set(Shape.ContentField.Name, Shape.Value);
        }

        [Shape(bindingType:"Translate")]
        public void Fields_Boolean(dynamic Display, dynamic Shape, TextWriter Output) {
            using (Display.ViewDataContainer.Model.Node(Shape.ContentPart))
            {
                using (Display.ViewDataContainer.Model.Node(Shape.ContentField)) {
                    bool? booleanValue = Shape.ContentField.Value;
                    Display.ViewDataContainer.Model.Value = booleanValue.HasValue ? booleanValue.Value : booleanValue;
                }
            }
        }

        [Shape(bindingType:"Translate")]
        public void Fields_Boolean__api__Flat(dynamic Display, dynamic Shape)
        {
            bool? booleanValue = Shape.ContentField.Value;
            Display.ViewDataContainer.Model.Set(Shape.ContentField.Name, booleanValue.HasValue ? booleanValue.Value : booleanValue);
        }

        [Shape(bindingType:"Translate")]
        public void Fields_DateTime(dynamic Display, dynamic Shape) {
            using (Display.ViewDataContainer.Model.Node(Shape.ContentPart))
            {
                using (Display.ViewDataContainer.Model.Node(Shape.ContentField)) {
                    Display.ViewDataContainer.Model.Value = Shape.ContentField.DateTime;
                }
            }
        }

        [Shape(bindingType:"Translate")]
        public void Fields_DateTime__api__Flat(dynamic Display, dynamic Shape)
        {
            Display.ViewDataContainer.Model.Set(Shape.ContentField.Name, Shape.ContentField.DateTime);
        }

        [Shape(bindingType:"Translate")]
        public void Fields_Numeric(dynamic Display, dynamic Shape) {
            using (Display.ViewDataContainer.Model.Node(Shape.ContentPart))
            {
                using (Display.ViewDataContainer.Model.Node(Shape.ContentField)) {
                    Display.ViewDataContainer.Model.Value = Shape.Value;
                }
            }
        }

        [Shape(bindingType:"Translate")]
        public void Fields_Numeric__api__Flat(dynamic Display, dynamic Shape)
        {
            Display.ViewDataContainer.Model.Set(Shape.ContentField.Name, Shape.Value);
        }

        [Shape(bindingType:"Translate")]
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

        [Shape(bindingType:"Translate")]
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

        [Shape(bindingType:"Translate")]
        public void Fields_MediaLibraryPicker(dynamic Display, dynamic Shape)
        {
            using (Display.ViewDataContainer.Model.Node(Shape.ContentPart))
            {
                Fields_MediaLibraryPicker__api__Flat(Display, Shape);
            }
        }

        [Shape(bindingType:"Translate")]
        public void Fields_MediaLibraryPicker__api__Flat(dynamic Display, dynamic Shape) {
            var field = Shape.ContentField;
            string name = field.DisplayName;
            var contents = field.MediaParts;
            var mediaShapes = new List<dynamic>();

            using (Display.ViewDataContainer.Model.List(Shape.ContentField)) {
                using (_alterations.Value.CreateScope("Translate")) {
                    foreach (var item in contents) {
                        Display( _contentManager.Value.BuildDisplay(item, "Summary"));
                    }     
                }
            }
        }

        [Shape(bindingType:"Translate")]
        public void Fields_Link(dynamic Display, dynamic Shape)
        {
            using (Display.ViewDataContainer.Model.Node(Shape.ContentPart))
            {
                Fields_Link__api__Flat(Display, Shape);
            }
        }

        [Shape(bindingType:"Translate")]
        public void Fields_Link__api__Flat(dynamic Display, dynamic Shape)
        {

            string name = Shape.ContentField.DisplayName;
            LinkFieldSettings settings = Shape.ContentField.PartFieldDefinition.Settings.GetModel<LinkFieldSettings>();
            string text = Shape.ContentField.Text;
            switch (settings.LinkTextMode)
            {
                case LinkTextMode.Static:
                    text = settings.StaticText;
                    break;
                case LinkTextMode.Url:
                    text = Shape.ContentField.Value;
                    break;
                case LinkTextMode.Optional:
                    if (String.IsNullOrWhiteSpace(text))
                    {
                        text = Shape.ContentField.Value;
                    }
                    break;
            }


            using (Display.ViewDataContainer.Model.Node(Shape.ContentField))
            {
                Display.ViewDataContainer.Model.Url = Shape.ContentField.Value;
                Display.ViewDataContainer.Model.Text = text;
                Display.ViewDataContainer.Model.Target = Shape.ContentField.Target;
            }
        }
    }
}
