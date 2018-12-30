using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public delegate BaseValue GetPropertyDelegate(Engine engine, BaseValue self);

    public class GetPropertyFunction : IGetProperty {
        public GetPropertyDelegate Property;

        public GetPropertyFunction (GetPropertyDelegate del) {
            Property = del;
        }

        public BaseValue Run(Engine engine, BaseValue self) {
            return Property.Invoke(engine, self);
        }
    }
}
