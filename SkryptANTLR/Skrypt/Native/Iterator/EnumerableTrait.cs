using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class EnumerableTrait : BaseTrait {
        public EnumerableTrait(Engine engine) : base(engine) {}

        public static BaseObject GetIterator(Engine engine, BaseObject self, Arguments arguments) => null;
        public static BaseObject Get(Engine engine, BaseObject self, Arguments arguments) => null;
        public static BaseObject IsInRange(Engine engine, BaseObject self, Arguments arguments) => null;
    }
}
