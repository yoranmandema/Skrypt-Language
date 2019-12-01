using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class ExceptionType : SkryptType {

        public ExceptionType(SkryptEngine engine) : base(engine) {
        }

        public SkryptInstance Construct(string message, string stackTrace) {
            var obj = new ExceptionInstance(Engine, message, stackTrace);

            obj.GetProperties(Template);
            obj.TypeObject = this;

            return obj;
        }

        public override SkryptInstance Construct(Arguments arguments) {
            return Construct(arguments.GetAs<StringInstance>(0).Value, arguments.GetAs<StringInstance>(1).Value);
        }
    }
}
