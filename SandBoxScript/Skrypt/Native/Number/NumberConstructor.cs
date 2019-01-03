using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class NumberConstructor : Constructor<NumberInstance> {
        public NumberConstructor(Engine engine) : base (engine) { }

        public NumberInstance Construct(double val) {
            var obj = new NumberInstance(_engine, val);

            obj.GetProperties(_template);

            return obj;
        }

        public override NumberInstance Construct(BaseValue[] arguments) {
            return Construct(((NumberInstance)arguments[0]).Value);
        }
    }
}
