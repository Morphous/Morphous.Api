using Orchard;
using Orchard.DisplayManagement.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Raven.Api
{
    public interface IBindingTypeDisplayAlterations : IDependency
    {
        IDisposable DisplayScope(string bindingType);
    }

    public class BindingTypeDisplayAlterations : IBindingTypeDisplayAlterations, IShapeDisplayEvents
    {
        private Stack<string> _displayBindingTypes = new Stack<string>();

        public IDisposable DisplayScope(string bindingtype) {
            return new BindingTypeAlterationsScope(_displayBindingTypes, bindingtype);
        }

        public void Displaying(ShapeDisplayingContext context) {
            if (_displayBindingTypes.Any()) {
                context.ShapeMetadata.BindingType = _displayBindingTypes.Peek();
            }
        }

        public void Displayed(ShapeDisplayedContext context) {

        }
    }

}