using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    class BooleanType : BaseType {
        public BooleanType(SkryptEngine engine) : base(engine) {
            Template = engine.TemplateMaker.CreateTemplate(typeof(BooleanInstance));
        }

        public SkryptInstance Construct(bool val) {
            var obj = new BooleanInstance(Engine, val);

            obj.GetProperties(Template);
            obj.TypeObject = this;

            return obj;
        }

        public override SkryptInstance Construct(Arguments arguments) {
            return Construct(arguments.GetAs<BooleanInstance>(0).Value);
        }
    }
}
