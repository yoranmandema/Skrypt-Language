using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class BooleanConstructor : Constructor<BooleanInstance> {
        public BooleanConstructor(Engine engine) : base (engine) { }

        public BooleanInstance Construct(bool val) {
            var obj = new BooleanInstance(_engine, val);

            obj.GetProperties(_template);

            return obj;
        }

        public override BooleanInstance Construct(BaseValue[] arguments) {
            return Construct(((BooleanInstance)arguments[0]).Value);
        }
    }
}
