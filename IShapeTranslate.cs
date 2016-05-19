using System.Collections.Generic;
using Orchard.DisplayManagement.Shapes;
using Orchard;
using Orchard.DisplayManagement.Descriptors;
using System.Web;

namespace Raven.Api.DisplayManagement {
    public interface IShapeTranslate : IDependency {
        object Display(Shape shape);
        object Display(object shape);
        IEnumerable<object> Display(IEnumerable<object> shapes);
    }
}
