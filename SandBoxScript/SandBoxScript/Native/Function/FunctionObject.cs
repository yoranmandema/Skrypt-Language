using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public class FunctionInstance : BaseInstance {
        public override string Name => "Function";

        public FunctionInstance(Engine engine, BaseDelegate function) : base(engine) {
            Function = new DelegateFunction(function);
        }

        public IFunction Function { get; set; }
    }
}
