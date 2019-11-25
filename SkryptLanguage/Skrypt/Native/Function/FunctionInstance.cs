using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class FunctionInstance : BaseInstance, IDefined {
        public string File { get; set; }
        public override string Name => "Function";
        public IFunction Function { get; set; }

        public FunctionInstance(Engine engine, MethodDelegate function) : base(engine) {
            Function = new NativeFunction(function);
        }

        public FunctionInstance(Engine engine, ScriptFunction function) : base(engine) {
            Function = function;
            File = function.File;
        }

        public BaseObject RunOnSelf (BaseObject self, params BaseObject[] args) {
            var arguments = new Arguments(args);

            return Function.Run(Engine, self, arguments);
        }

        public BaseObject Run (params BaseObject[] args) {
            var arguments = new Arguments(args);

            return Function.Run(Engine, null, arguments);
        }
    }
}
