using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class NumberType : BaseType {
        private readonly Template _template;

        public NumberType(Engine engine) : base(engine) {
            _template = engine.templateMaker.CreateTemplate(typeof(NumberInstance));
        }

        public static BaseValue Parse(Engine engine, BaseValue self, Arguments input) {
            var value = double.Parse(input.ToString(), System.Globalization.CultureInfo.InvariantCulture);

            return engine.CreateNumber(value);
        }

        public BaseInstance Construct(double val) {
            var obj = new NumberInstance(Engine, val);

            obj.GetProperties(_template);
            obj.TypeObject = this;

            return obj;
        }

        public override BaseInstance Construct(Arguments arguments) {
            return Construct(arguments.GetAs<NumberInstance>(0).Value);
        }
    }
}
