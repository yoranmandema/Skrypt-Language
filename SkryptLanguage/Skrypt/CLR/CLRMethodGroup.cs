using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Skrypt.CLR {
    public class CLRMethodGroup : IFunction {
        public string Name { get; set; }
        public CLRFunction[] functions;

        public BaseObject Run(Engine engine, BaseObject self, Arguments arguments) {
            BaseObject result = null;

            foreach (var function in functions) {
                var args = function.ConvertArguments(arguments, out bool isValid);

                if (!isValid) continue;

                var res = function.del.DynamicInvoke(args);

                result = CLRTypeConverter.ConvertToSkryptObject(engine, res);
            }

            return result;
        }
    }
}
