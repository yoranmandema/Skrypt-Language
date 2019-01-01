using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public class FunctionInstance : BaseInstance {
        public override string Name => "Function";
        public IFunction Function { get; set; }

        public FunctionInstance(Engine engine, MethodDelegate function) : base(engine) {
            Function = new NativeFunction(function);
        }

        public FunctionInstance(Engine engine, ScriptFunction function) : base(engine) {
            Function = function;
        }
    }
}
