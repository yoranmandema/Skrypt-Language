using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Skrypt.CLR {
    public class CLRMethodGroup : IFunction {
        public string Name { get; set; }
        public CLRMethod[] Functions { get; set; }

        public BaseObject Run(Engine engine, BaseObject self, Arguments arguments) {
            BaseObject result = null;
            bool validArguments = false;

            foreach (var function in Functions) {
                var args = function.ConvertArguments(arguments, out bool isValid);

                if (isValid) {
                    validArguments = isValid;

                    var res = function.del.DynamicInvoke(args);

                    result = CLRTypeConverter.ConvertToSkryptObject(engine, res);

                    break;
                }
            }

            if (!validArguments) {
                var argString = "";

                foreach (var arg in arguments.Values) argString += arg.GetType().Name + " ";

                throw new ArgumentException($"No method found for arguments {argString}");
            }

            return result;
        }
    }
}
