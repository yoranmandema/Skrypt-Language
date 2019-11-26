using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class GetPropertyInstance : SkryptInstance {
        public override string Name => "GetProperty";
        public IGetProperty Property;

        public GetPropertyInstance(SkryptEngine engine, GetPropertyDelegate property) : base(engine) {
            Property = new GetPropertyFunction(property);
        }
    }
}
