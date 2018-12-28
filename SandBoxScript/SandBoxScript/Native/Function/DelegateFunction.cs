using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public delegate BaseValue BaseDelegate(Engine engine, BaseValue self, Arguments input);

    public class DelegateFunction : IFunction {
        public BaseDelegate Function;

        public DelegateFunction (BaseDelegate function) {
            Function = function;
        }

        public BaseValue Run(Engine engine, BaseValue self, Arguments args) {
            return Function.Invoke(engine, self, args);
        }
    }
}
