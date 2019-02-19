using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class IteratorInstance : BaseInstance {
        public int index;
        public BaseObject Enumerable;

        public IteratorInstance(Engine engine) : base(engine) {}

        public static BaseObject Current(Engine engine, BaseObject self) {
            var iterator = self as IteratorInstance;
            var value = (iterator.Enumerable.Members["Get"].Value as FunctionInstance).RunOnSelf(iterator.Enumerable, engine.CreateNumber(iterator.index));

            return value;
        }

        public static BaseObject Next (Engine engine, BaseObject self, Arguments arguments) {
            var iterator = self as IteratorInstance;

            iterator.index++;

            return (iterator.Enumerable.Members["IsInRange"].Value as FunctionInstance).RunOnSelf(iterator.Enumerable, engine.CreateNumber(iterator.index));
        }

        public static BaseObject Reset(Engine engine, BaseObject self, Arguments arguments) {
            var iterator = self as IteratorInstance;

            iterator.index = 0;

            return null;
        }
    }
}
