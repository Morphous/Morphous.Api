using Orchard;
using Orchard.DisplayManagement;
using Orchard.DisplayManagement.Descriptors;
using Orchard.DisplayManagement.Implementation;
using Orchard.DisplayManagement.Shapes;
using Raven.Api.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Raven.Api.DisplayManagement {
    public class ShapeTranslate : ApiShapesBase, IShapeTranslate {

        private readonly IDisplayHelperFactory _displayHelperFactory;
        private readonly IWorkContextAccessor _workContextAccessor;
        private readonly HttpContextBase _httpContextBase;
        private readonly RequestContext _requestContext;

        public ShapeTranslate(
            IDisplayHelperFactory displayHelperFactory,
            IWorkContextAccessor workContextAccessor,
            HttpContextBase httpContextBase,
            RequestContext requestContext) {

            _displayHelperFactory = displayHelperFactory;
            _workContextAccessor = workContextAccessor;
            _httpContextBase = httpContextBase;
            _requestContext = requestContext;
        }

        public object Display(Shape shape) {
            return Display((object)shape);
        }

        public object Display(object shape) {
            var viewContext = new ViewContext {
                HttpContext = _httpContextBase,
                RequestContext = _requestContext
            };

            viewContext.RouteData.DataTokens["IWorkContextAccessor"] = _workContextAccessor;
            var display = _displayHelperFactory.CreateHelper(viewContext, new ViewDataContainer());
         
            ((DisplayHelper)display).ShapeExecute(shape);

            return display.ViewDataContainer.Model.Properties;
        }

        public IEnumerable<object> Display(IEnumerable<object> shapes) {
            return shapes.Select(s => Display(s));
        }

        public class ViewDataContainer : IViewDataContainer {
            
            public ViewDataDictionary ViewData { get; set; }
            public dynamic Model { get; set; }
            public dynamic CurrentNode { get; set; }

            public ViewDataContainer() {
                Model = new ViewModelBuilder();
                CurrentNode = Model;
                ViewData = new ViewDataDictionary();
            }
        }
    }
}