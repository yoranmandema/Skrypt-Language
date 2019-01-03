using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    class StringConstructor : Constructor<StringInstance> {
        public StringConstructor(Engine engine) : base(engine) { }

        public StringInstance Construct(string val) {
            var obj = new StringInstance(_engine, val);

            obj.GetProperties(_template);

            return obj;
        }

        public override StringInstance Construct(BaseValue[] arguments) {
            return Construct(arguments[0].ToString());
        }
    }
}
