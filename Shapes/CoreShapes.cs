using Orchard;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.DisplayManagement;
using Orchard.DisplayManagement.Descriptors;
using Orchard.DisplayManagement.Shapes;
using Orchard.Environment;
using Orchard.Localization;
using Orchard.Mvc;
using Orchard.Mvc.Html;
using Orchard.UI.Resources;
using Raven.Api.Services;
using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Web.Http.Routing;
using Raven.Api.Extensions;
using System.Net.Http;

namespace Raven.Api.Shapes {
    public class CoreShapes : ApiShapesBase, IShapeTableProvider {
        private readonly Work<WorkContext> _workContext;

        public CoreShapes(
            Work<WorkContext> workContext,
            Work<IHttpContextAccessor> httpContextAccessor
            ) {
            _workContext = workContext;

            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }
        public void Discover(ShapeTableBuilder builder) {

        }

        [Shape(bindingType:"Translate")]
        public void Content(dynamic Display, dynamic Shape) {
            using (Display.ViewDataContainer.Model.Node("Content")) {

                    Display.ViewDataContainer.Model.ContentType = Shape.ContentItem.ContentType;
                    Display.ViewDataContainer.Model.DisplayType = Shape.Metadata.DisplayType;

             

                    if (Shape.Meta != null)
                    {
                        Display(Shape.Meta);
                    }
         

                Display(Shape.Header);
                Display(Shape.Content);
                if (Shape.Footer != null) {
                    Display(Shape.Footer);
                }
            }
        }

        [Shape(bindingType:"Translate")]
        public void ContentZone(dynamic Display, dynamic Shape) {
            foreach (var item in (IEnumerable<dynamic>)Order(Shape)) {
                Display(item);
            }
        }

        [Shape(bindingType:"Translate")]
        public void DocumentZone(dynamic Display, dynamic Shape) {
            foreach (var item in (IEnumerable<dynamic>)Order(Shape)) {
                Display(item);
            }
        }

        [Shape(bindingType:"Translate")]
        public void Pager(dynamic Shape, dynamic Display,
            int Page,
            int PageSize,
            double TotalItemCount,
            int? Quantity,
            object FirstText,
            object PreviousText,
            object NextText,
            object LastText,
            object GapText,
            string PagerId
            // parameter omitted to workaround an issue where a NullRef is thrown
            // when an anonymous object is bound to an object shape parameter
            /*object RouteValues*/) {

            using (Display.ViewDataContainer.Model.Node(Shape.ContentPart ?? Shape.Term)) {
                Pager__api__Flat(Shape, Display,
                     Page,
                     PageSize,
                     TotalItemCount,
                     Quantity,
                     FirstText,
                     PreviousText,
                     NextText,
                     LastText,
                     GapText,
                     PagerId);
                                }
        }

        [Shape(bindingType:"Translate")]
        public void Pager__api__Flat(dynamic Shape, dynamic Display,
            int Page,
            int PageSize,
            double TotalItemCount,
            int? Quantity,
            object FirstText,
            object PreviousText,
            object NextText,
            object LastText,
            object GapText,
            string PagerId
            // parameter omitted to workaround an issue where a NullRef is thrown
            // when an anonymous object is bound to an object shape parameter
            /*object RouteValues*/) {

            var currentPage = Page;
            if (currentPage < 1)
                currentPage = 1;

            var pageSize = PageSize;
            if (pageSize < 1)
                pageSize = _workContext.Value.CurrentSite.PageSize;

            var numberOfPagesToShow = Quantity ?? 0;
            if (Quantity == null || Quantity < 0)
                numberOfPagesToShow = 7;

            var totalPageCount = (int)Math.Ceiling(TotalItemCount / pageSize);

            var pageKey = String.IsNullOrEmpty(PagerId) ? "page" : PagerId;

            using (Display.ViewDataContainer.Model.Node("Pager")) {
                Display.ViewDataContainer.Model.CurrentPage = currentPage;
                Display.ViewDataContainer.Model.TotalPageCount = totalPageCount;
                Display.ViewDataContainer.Model.TotalItemCount = TotalItemCount;
                Display.ViewDataContainer.Model.PageSize = pageSize;
                Display.ViewDataContainer.Model.PageKey = pageKey;

                if (Shape.ContentPart != null && Shape.ContentPart.Record.PagerSuffix != null) {
                    Display.ViewDataContainer.Model.PageSizeKey = "pageSize" + Shape.ContentPart.Record.PagerSuffix;
                }
            }
        }

        [Shape(bindingType:"Translate")]
        public void Parts_Common_Metadata(dynamic Display, dynamic Shape) {
            
            using (Display.ViewDataContainer.Model.Node("Metadata"))
            {
                Parts_Common_Metadata__api__Flat(Display, Shape);
            }

        }

        [Shape(bindingType:"Translate")]
        public void Parts_Common_Metadata_Summary(dynamic Display, dynamic Shape) {
            Parts_Common_Metadata(Display, Shape);
        }

        [Shape(bindingType:"Translate")]
        public void Parts_Common_Metadata__api__Flat(dynamic Display, dynamic Shape)
        {
            System.Web.Mvc.UrlHelper urlHelper = new System.Web.Mvc.UrlHelper(Display.ViewContext.RequestContext);

            Display.ViewDataContainer.Model.Id = Shape.ContentPart.Id;
            Display.ViewDataContainer.Model.ResourceUrl = urlHelper.ItemApiGet((IContent)Shape.ContentPart);
            Display.ViewDataContainer.Model.CreatedUtc = Shape.ContentPart.CreatedUtc;
            Display.ViewDataContainer.Model.PublishedUtc = Shape.ContentPart.PublishedUtc;
        }

        [Shape(bindingType:"Translate")]
        public void Parts_Common_Metadata_Summary__api__Flat(dynamic Display, dynamic Shape)
        {
            Parts_Common_Metadata__api__Flat(Display, Shape);
        }

        [Shape(bindingType:"Translate")]
        public void Parts_Title(dynamic Display, dynamic Shape) {
            using (Display.ViewDataContainer.Model.Node(Shape.ContentPart))
            {
                Parts_Title_Summary__api__Flat(Display, Shape);
              //  Display.ViewDataContainer.Model.Title = Shape.Title;
            }
        }

        [Shape(bindingType:"Translate")]
        public void Parts_Title_Summary(dynamic Display, dynamic Shape) {
            using (Display.ViewDataContainer.Model.Node(Shape.ContentPart))
            {
                Parts_Title_Summary__api__Flat(Display, Shape);
            }
        }

        [Shape(bindingType:"Translate")]
        public void Parts_Title__api__Flat(dynamic Display, dynamic Shape)
        {
                Display.ViewDataContainer.Model.Title = Shape.Title;
        }

        [Shape(bindingType:"Translate")]
        public void Parts_Title_Summary__api__Flat(dynamic Display, dynamic Shape)
        {
            Display.ViewDataContainer.Model.Title = Shape.Title;
        }

        [Shape(bindingType:"Translate")]
        public void Parts_Common_Body(dynamic Display, dynamic Shape) {
            using (Display.ViewDataContainer.Model.Node(Shape.ContentPart))
            {
                Display.ViewDataContainer.Model.Html = Shape.Html.ToString();
            }
        }

        [Shape(bindingType:"Translate")]
        public void Parts_Common_Body_Summary(dynamic Display, dynamic Shape) {
            using (Display.ViewDataContainer.Model.Node(Shape.ContentPart))
            {
                Display.ViewDataContainer.Model.Html = Shape.Html.ToString();
            }
        }

        [Shape(bindingType:"Translate")]
        public void Parts_Common_Body__api__Flat(dynamic Display, dynamic Shape)
        {
            Display.ViewDataContainer.Model.Body = Shape.Html.ToString();
        }

        [Shape(bindingType:"Translate")]
        public void Parts_Common_Body_Summary__api__Flat(dynamic Display, dynamic Shape)
        {
            Display.ViewDataContainer.Model.Body = Shape.Html.ToString();
        }

        [Shape(bindingType:"Translate")]
        public void Parts_Contents_Publish(dynamic Display, dynamic Shape) {

        }

        [Shape(bindingType:"Translate")]
        public void List(dynamic Display, dynamic Shape) {
            using (Display.ViewDataContainer.Model.Node(Shape.ContentPart ?? "Items")) {
                List__api__Flat(Display,Shape);
            }
        }

        [Shape(bindingType:"Translate")]
        public void List__api__Flat(dynamic Display, dynamic Shape) {
            using (Display.ViewDataContainer.Model.List(Shape.Name ?? "Items")) {
                foreach (var item in ((IEnumerable<dynamic>)Shape.Items)) {
                    Display(item);
                }
            }
        }

        [Shape(bindingType:"Translate")]
        public void Parts_Container_Contained(dynamic Display, dynamic Shape) {
            using (Display.ViewDataContainer.Model.Node(Shape.ContentPart)) {
                Parts_Container_Contained__api__Flat(Display, Shape);
            }
        }

        [Shape(bindingType:"Translate")]
        public void Parts_Container_Contained__api__Flat(dynamic Display, dynamic Shape) {
            Shape.List.Name = "ChildItems";
            Display(Shape.List);
        }
    }
}
