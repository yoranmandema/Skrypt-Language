using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public delegate BaseObject MethodDelegate(Engine engine, BaseObject self, Arguments input);

    public class NativeFunction : IFunction {
        public MethodDelegate Function;

        public NativeFunction (MethodDelegate function) {
            Function = function;
        }

        public BaseObject Run(Engine engine, BaseObject self, Arguments args) {
            return Function.Invoke(engine, self, args);
        }
    }
}
