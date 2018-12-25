using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    sealed class NumberConstructor : IConstructor<NumberInstance> {
        private readonly Engine _engine;

        public NumberConstructor(Engine engine) {
            _engine = engine;
        }

        public NumberInstance Construct(double val) {
            var obj = new NumberInstance(_engine, val);

            obj.GetProperties(_engine.NumberTemplate.Members);

            return obj;
        }

        public NumberInstance Construct(BaseValue[] arguments) {
            return Construct(((NumberInstance)arguments[0]).Value);
        }
    }
}
