using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public delegate SkryptObject MethodDelegate(SkryptEngine engine, SkryptObject self, Arguments input);

    public class NativeFunction : IFunction {
        public MethodDelegate Function;

        public NativeFunction (MethodDelegate function) {
            Function = function;
        }

        public SkryptObject Run(SkryptEngine engine, SkryptObject self, Arguments args) {
            return Function.Invoke(engine, self, args);
        }
    }
}
