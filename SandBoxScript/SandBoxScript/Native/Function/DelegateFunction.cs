using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public delegate BaseObject BaseDelegate(Engine engine, BaseObject self, BaseObject[] input);

    public class DelegateFunction : IFunction {
        public BaseDelegate Function;

        public BaseObject Run(Engine engine, BaseObject self, BaseObject[] args) {
            return Function.Invoke(engine, self, args);
        }
    }
}
