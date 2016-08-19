using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Orchard;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Mvc.Filters;
using Orchard.UI.Notify;
using Orchard.WebApi.Filters;

namespace Morphous.Api.Filters {
    public class NotifyFilterAttribute :  ActionFilterAttribute {
        public NotifyFilterAttribute() {
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext) {
            base.OnActionExecuted(actionExecutedContext);

            var workContext = actionExecutedContext.ActionContext.ControllerContext.GetWorkContext();
            var notifier = workContext.Resolve<INotifier>();

            // Don't touch the response if there's no work to perform.
            if (!notifier.List().Any())
                return;

            // Some error responses have no content. Create a new empty content object for the OrchardMetaData to be added into.
            if (actionExecutedContext.Response.Content == null) {
                actionExecutedContext.Response.Content = new ObjectContent<Dictionary<string, object>>(new Dictionary<string, object>(), new JsonMediaTypeFormatter());
            }

            Dictionary<string, object> model = null;
            if(actionExecutedContext.Response.TryGetContentValue(out model)) {
                var orchardMetaData = new OrchardMetaData();

                var messageEntries = notifier.List().ToList();
                orchardMetaData.Messages = messageEntries.Select(m => m.Message.ToString()).ToList();

                model.Add("OrchardMetaData", orchardMetaData);
            }            
        }
        
        private class OrchardMetaData {
            public IList<string> Messages { get; set; }
        } 
    }
}