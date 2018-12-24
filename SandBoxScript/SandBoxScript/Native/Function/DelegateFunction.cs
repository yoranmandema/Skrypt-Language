using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public delegate BaseObject BaseDelegate(BaseObject[] input);

    public class DelegateFunction : IFunction {
        public BaseDelegate Function;

        public BaseObject Run(BaseObject[] args) {
            return Function.Invoke(args);
        }
    }
}
