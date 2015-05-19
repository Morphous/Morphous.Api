using System.Collections.Generic;
using Orchard.DisplayManagement.Shapes;
using Orchard;

namespace Raven.Api.DisplayManagement {
    public interface IShapeTranslate : IDependency {
        object Display(Shape shape);
        object Display(object shape);
        IEnumerable<object> Display(IEnumerable<object> shapes);
    }
}
