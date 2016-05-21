using Orchard;
using Orchard.DisplayManagement.Descriptors;
using Orchard.DisplayManagement.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Raven.Api
{
    public interface IBindingTypeCreateAlterations : IDependency {
        IDisposable CreateScope(string bindingType);
    }

    public class BindingTypeCreateAlterations : IBindingTypeCreateAlterations, IShapeFactoryEvents
    {
        private Stack<string> _createBindingTypes = new Stack<string>();

        public IDisposable CreateScope(string bindingType) {
            return new BindingTypeAlterationsScope(_createBindingTypes, bindingType);
        }


        public void Creating(ShapeCreatingContext context) {

        }

        public void Created(ShapeCreatedContext context) {
            if (_createBindingTypes.Any()) {
                context.Shape.Metadata.BindingType = _createBindingTypes.Peek();
            }
        }
    }
}
