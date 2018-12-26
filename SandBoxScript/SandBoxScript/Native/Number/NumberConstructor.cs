using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    sealed class NumberConstructor : IConstructor<NumberInstance> {
        private readonly Engine _engine;
        private readonly Template _template;

        public NumberConstructor(Engine engine) {
            _engine = engine;

            _template = _engine.templateMaker.CreateTemplate(typeof(NumberInstance));
        }

        public NumberInstance Construct(double val) {
            var obj = new NumberInstance(_engine, val);

            obj.GetProperties(_template);

            return obj;
        }

        public NumberInstance Construct(BaseValue[] arguments) {
            return Construct(((NumberInstance)arguments[0]).Value);
        }
    }
}
