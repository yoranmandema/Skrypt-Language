using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class IteratorType : BaseType {
        public IteratorType(Engine engine) : base(engine) {
            Template = engine.templateMaker.CreateTemplate(typeof(IteratorInstance));
        }

        public BaseInstance Construct(BaseObject[] values) {
            var obj = new IteratorInstance(Engine);

            obj.GetProperties(Template);
            obj.TypeObject = this;
            obj.Enumerable = values[0];

            return obj;
        }

        public override BaseInstance Construct(Arguments arguments) {
            return Construct(arguments.Values);
        }
    }
}
