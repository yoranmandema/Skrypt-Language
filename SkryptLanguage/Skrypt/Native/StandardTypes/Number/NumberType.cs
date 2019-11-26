using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class NumberType : BaseType {
        public NumberType(SkryptEngine engine) : base(engine) {
            Template = engine.TemplateMaker.CreateTemplate(typeof(NumberInstance));
        }

        public static SkryptObject Parse(SkryptEngine engine, SkryptObject self, Arguments input) {
            var value = double.Parse(input[0].ToString(), System.Globalization.CultureInfo.InvariantCulture);

            return engine.CreateNumber(value);
        }

        public static SkryptObject ParseInt(SkryptEngine engine, SkryptObject self, Arguments input) {
            var value = int.Parse(input[0].ToString(), System.Globalization.CultureInfo.InvariantCulture);

            return engine.CreateNumber(value);
        }

        public SkryptInstance Construct(double val) {
            var obj = new NumberInstance(Engine, val);

            obj.GetProperties(Template);
            obj.TypeObject = this;

            return obj;
        }

        public override SkryptInstance Construct(Arguments arguments) {
            return Construct(arguments.GetAs<NumberInstance>(0).Value);
        }
    }
}
