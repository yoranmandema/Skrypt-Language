using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    class StringType : BaseType {
        public StringType(Engine engine) : base(engine) {
            Template = engine.templateMaker.CreateTemplate(typeof(StringInstance));
        }

        public BaseInstance Construct(string val) {
            var obj = new StringInstance(Engine, val);

            obj.GetProperties(Template);
            obj.TypeObject = this;

            return obj;
        }

        public override BaseInstance Construct(Arguments arguments) {
            return Construct(arguments[0].ToString());
        }
    }
}