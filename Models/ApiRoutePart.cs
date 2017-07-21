using Orchard.ContentManagement;
using Orchard.Environment.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Morphous.Api.Models
{
    [OrchardFeature("Morphous.Api.Routes")]
    public class ApiRoutePart : ContentPart
    {
        public string RouteTemplate {
            get { return this.Retrieve(r => r.RouteTemplate); }
            set { this.Store(r => r.RouteTemplate, value); }
        }

        public string Name
        {
            get { return this.Retrieve(r => r.Name); }
            set { this.Store(r => r.Name, value); }
        }
    }
}