using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public delegate BaseObject GetPropertyDelegate(Engine engine, BaseObject self);

    public class GetPropertyFunction : IGetProperty {
        public GetPropertyDelegate Property;

        public GetPropertyFunction (GetPropertyDelegate del) {
            Property = del;
        }

        public BaseObject Run(Engine engine, BaseObject self) {
            return Property.Invoke(engine, self);
        }
    }
}
