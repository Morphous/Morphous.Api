using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Raven.Api
{
    public class BindingTypeAlterationsScope : IDisposable
    {
        private Stack<string> _bindingTypes;

        public BindingTypeAlterationsScope(Stack<string> bindingTypes, string bindingType) {
            _bindingTypes = bindingTypes;
            _bindingTypes.Push(bindingType);
        }

        public void Dispose() {
            _bindingTypes.Pop();
        }
    }
}