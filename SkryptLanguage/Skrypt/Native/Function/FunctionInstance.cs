using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class FunctionInstance : SkryptInstance, IDefined {
        public string File { get; set; }
        public override string Name => "Function";
        public IFunction Function { get; set; }

        public FunctionInstance(SkryptEngine engine, MethodDelegate function) : base(engine) {
            Function = new NativeFunction(function);
        }

        public FunctionInstance(SkryptEngine engine, ScriptFunction function) : base(engine) {
            Function = function;
            File = function.File;
        }

        public FunctionInstance(SkryptEngine engine, IFunction function) : base(engine) {
            Function = function;
        }


        public SkryptObject RunOnSelf (SkryptObject self, params SkryptObject[] args) {
            var arguments = new Arguments(args);

            return Function.Run(Engine, self, arguments);
        }

        public SkryptObject Run (params SkryptObject[] args) {
            var arguments = new Arguments(args);

            return Function.Run(Engine, null, arguments);
        }
    }
}
