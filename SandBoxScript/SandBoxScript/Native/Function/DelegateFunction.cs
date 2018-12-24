using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public class DelegateFunction : IFunction {
        public Delegate Function;

        public BaseObject Run(params BaseObject[] args) {
            return (BaseObject)Function.DynamicInvoke(args);
        }
    }
}
