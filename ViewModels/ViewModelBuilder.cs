using Orchard.DisplayManagement;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace Raven.Api.ViewModels {

    public class ViewModelNode : Dictionary<string, object> {
        public ViewModelNode() { 
           
        }
    }

    public class ViewModelBuilder : DynamicObject {

        public IDictionary<string, object> Properties { get { return _nodeStack.Reverse().First(); } }

        private readonly Stack<dynamic> _nodeStack;

        public ViewModelBuilder() {
            _nodeStack = new Stack<object>();
        }

        public object Pop() {

            if (_nodeStack.Count > 1) {
                return _nodeStack.Pop();
            }
            else {
                return null;
            }

        }

        public void Push(object node) {
            _nodeStack.Push(node);
        }

        public CaptureScope Node(dynamic shape)
        {
            return Node(shape.ContentPart.PartDefinition.Name);
        }

        public CaptureScope Node(string name) {

            var newNode = new ViewModelNode();
            return CreateNode(name, newNode);

        }

        public CaptureScope List(string name) {

            var newNode = new List<dynamic>();
            return CreateNode(name, newNode);

        }

        public void Set(string name, object value) {
            _nodeStack.Peek()[name] = value;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result) {
            return TryGetMemberImpl(binder.Name, out result);
        }

        protected virtual bool TryGetMemberImpl(string name, out object result) {

            var currentNode = _nodeStack.Peek();

            if (currentNode.GetType() != typeof(List<dynamic>) && currentNode.ContainsKey(name)) {
                result = currentNode[name];
                return true;
            }

            result = null;
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value) {
            return TrySetMemberImpl(binder.Name, value);
        }

        protected bool TrySetMemberImpl(string name, object value) {

            _nodeStack.Peek()[name] = value;
            return true;

        }

        protected CaptureScope CreateNode(string name, object newNode) {

            if (_nodeStack.Count == 0) {
                return new CaptureScope(this, newNode);
            }

            var currentNode = _nodeStack.Peek();

            if (currentNode.GetType() == typeof(List<dynamic>)){
                currentNode.Add(newNode);
            }
            else if (currentNode.ContainsKey(name)) {
                return new CaptureScope(this, currentNode[name]);
            }
            else {
                _nodeStack.Peek().Add(name, newNode);
            }

            return new CaptureScope(this, newNode);

        }


        public class CaptureScope : IDisposable {

            private ViewModelBuilder _builder;

            public CaptureScope(ViewModelBuilder builder, object node) {

                _builder = builder;
                _builder.Push(node);

            }

            public void Dispose() {
                _builder.Pop();
            }

        }
    }
}