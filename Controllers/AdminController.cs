using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Orchard.ContentManagement;
using Orchard.UI.Notify;
using System.Reflection;
using Orchard.Data;
using Orchard.Core.Contents;
using Orchard.ContentManagement.MetaData;
using Orchard.Settings;
using Orchard.Localization.Services;
using Orchard;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Logging;
using Raven.Api.Extensions;


namespace Raven.Api.Controllers {
    [CamelCaseController]
    public class AdminController : ApiController, IUpdateModel {
        

        private JObject _model;

        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly ITransactionManager _transactionManager;
        private readonly ISiteService _siteService;
        private readonly ICultureManager _cultureManager;
        private readonly ICultureFilter _cultureFilter;

        public AdminController(
            IOrchardServices orchardServices,
            IContentManager contentManager,
            IContentDefinitionManager contentDefinitionManager,
            ITransactionManager transactionManager,
            ISiteService siteService,
            IShapeFactory shapeFactory,
            ICultureManager cultureManager,
            ICultureFilter cultureFilter) {
            Services = orchardServices;
            _contentManager = contentManager;
            _contentDefinitionManager = contentDefinitionManager;
            _transactionManager = transactionManager;
            _siteService = siteService;
            _cultureManager = cultureManager;
            _cultureFilter = cultureFilter;

            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;
            Shape = shapeFactory;
        }

        dynamic Shape { get; set; }
        public IOrchardServices Services { get; private set; }
        public Localizer T { get; set; }
        public ILogger Logger { get; set; }

        // GET api/<controller>
        public string[] Get() {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id) {
            return "value";
        }

        // POST api/<controller>
        public IHttpActionResult Post(JObject viewmodel) {

            _model = viewmodel;

            var contentItem = _contentManager.New("TeamMember");

            if (!Services.Authorizer.Authorize(Permissions.EditContent, contentItem, T("Couldn't create content")))
                return Ok();

            _contentManager.Create(contentItem, VersionOptions.Draft);

            var model = _contentManager.UpdateEditor(contentItem, this);

            if (!ModelState.IsValid) {
                _transactionManager.Cancel();
                return BadRequest(ModelState);
            }

            _contentManager.Publish(contentItem);

            Services.Notifier.Information(string.IsNullOrWhiteSpace(contentItem.TypeDefinition.DisplayName)
                ? T("Your content has been created.")
                : T("Your {0} has been created.", contentItem.TypeDefinition.DisplayName));

            return Ok(Services.Notifier.List().Select(n => n.Message.Text));

            
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value) {
        }

        // DELETE api/<controller>/5
        public void Delete(int id) {
        }

        public bool TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties) where TModel : class {

            JToken node = null;
            // _model.TryGetValue(prefix,out node);

            prefix = string.Join(".", prefix.Split('.').Select(s => s.ToCamelCase()));

            node = _model.SelectToken(prefix);
            if(node != null)
            {
                var props = typeof(TModel).GetProperties(BindingFlags.Public |
                      BindingFlags.NonPublic |
                      BindingFlags.Instance |
                      BindingFlags.DeclaredOnly);

                    if (includeProperties != null) {
                        props = props.Where(p => includeProperties.Contains(p.Name)).ToArray();
                    }
                    else if(excludeProperties != null) {
                        props = props.Where(p => !excludeProperties.Contains(p.Name)).ToArray();
                    }

                    foreach (var prop in props) {
                        var token = node[prop.Name.ToCamelCase()];
                        if (token != null) {
                            prop.SetValue(model, token.ToObject(prop.PropertyType));
                        }

                        //Validate<TModel>(model);
            }
           
            }
            
            return true;
        }

        public void AddModelError(string key, Orchard.Localization.LocalizedString errorMessage) {
           this.AddModelError(key, errorMessage);
        }
    }
}