using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    class BooleanType : BaseType {
        public BooleanType(Engine engine) : base(engine) {
            Template = engine.templateMaker.CreateTemplate(typeof(BooleanInstance));
        }

        public BaseInstance Construct(bool val) {
            var obj = new BooleanInstance(Engine, val);

            obj.GetProperties(Template);
            obj.TypeObject = this;

            return obj;
        }

        public override BaseInstance Construct(Arguments arguments) {
            return Construct(arguments.GetAs<BooleanInstance>(0).Value);
        }
    }
}
