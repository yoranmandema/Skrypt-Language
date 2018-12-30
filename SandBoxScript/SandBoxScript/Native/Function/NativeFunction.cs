using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public delegate BaseValue MethodDelegate(Engine engine, BaseValue self, Arguments input);

    public class NativeFunction : IFunction {
        public MethodDelegate Function;

        public NativeFunction (MethodDelegate function) {
            Function = function;
        }

        public BaseValue Run(Engine engine, BaseValue self, Arguments args) {
            return Function.Invoke(engine, self, args);
        }
    }
}
