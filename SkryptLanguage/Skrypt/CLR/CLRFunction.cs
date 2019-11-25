using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Skrypt.CLR {
    public class CLRFunction : IFunction {
        public string Name { get; set; }
        public CLRMethod Function { get; set; }

        public CLRFunction (Delegate del) {
            Function = CLRTypeConverter.CreateCLRFunction(del);
        }

        public BaseObject Run(Engine engine, BaseObject self, Arguments arguments) {
            BaseObject result = null;

            var isValid = Function.HasValidArguments(arguments);

            if (isValid) {
                var args = Function.ConvertArguments(arguments);
                var res = Function.del.DynamicInvoke(args);

                result = CLRTypeConverter.ConvertToSkryptObject(engine, res);
            }

            if (!isValid) {
                var argString = "";

                foreach (var arg in arguments.Values) argString += arg.GetType().Name + " ";

                throw new ArgumentException($"No method found for arguments {argString}");
            }

            return result;
        }
    }
}
