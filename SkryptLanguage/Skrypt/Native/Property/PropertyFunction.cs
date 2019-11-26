using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public delegate SkryptObject GetPropertyDelegate(SkryptEngine engine, SkryptObject self);

    public class GetPropertyFunction : IGetProperty {
        public GetPropertyDelegate Property;

        public GetPropertyFunction (GetPropertyDelegate del) {
            Property = del;
        }

        public SkryptObject Run(SkryptEngine engine, SkryptObject self) {
            return Property.Invoke(engine, self);
        }
    }
}
