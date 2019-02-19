using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class IteratorTrait : BaseTrait {
        public IteratorTrait(Engine engine) : base(engine) {
        }

        public static BaseObject Current    (Engine engine, BaseObject self, Arguments arguments) => null;
        public static BaseObject Next       (Engine engine, BaseObject self, Arguments arguments) => null;
        public static BaseObject HasNext    (Engine engine, BaseObject self, Arguments arguments) => null;
        public static BaseObject Reset      (Engine engine, BaseObject self, Arguments arguments) => null;
    }
}
