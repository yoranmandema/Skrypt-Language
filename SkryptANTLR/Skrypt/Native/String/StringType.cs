using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    class StringType : BaseType {
        private readonly Template _template;

        public StringType(Engine engine) : base(engine) {
            _template = engine.templateMaker.CreateTemplate(typeof(StringInstance));
        }

        public BaseInstance Construct(string val) {
            var obj = new StringInstance(Engine, val);

            obj.GetProperties(_template);

            return obj;
        }

        public override BaseInstance Construct(Arguments arguments) {
            return Construct(arguments[0].ToString());
        }
    }
}