using Morphous.Api.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Morphous.Api.Drivers
{
    public class ApiRoutePartDriver : ContentPartDriver<ApiRoutePart>
    {
        protected override DriverResult Display(ApiRoutePart part, string displayType, dynamic shapeHelper) {
            return null;
        }

        protected override DriverResult Editor(ApiRoutePart part, dynamic shapeHelper) {
            return ContentShape("Parts_ApiRoute_Edit",
               () => shapeHelper.EditorTemplate(TemplateName: "Parts.ApiRoute.Edit", Model: part, Prefix: Prefix));
        }

        protected override DriverResult Editor(ApiRoutePart part, IUpdateModel updater, dynamic shapeHelper) {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}