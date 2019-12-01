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

        public CLRFunction (SkryptEngine engine, Delegate del) {
            Function = CLRTypeConverter.CreateCLRFunction(engine, del);
        }

        public SkryptObject Run(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            SkryptObject result = null;

            var isValid = Function.HasValidArguments(arguments);

            if (isValid) {
                var args = Function.ConvertArguments(arguments);
                var res = Function.del.DynamicInvoke(args);

                if (engine.ImportTypeMappers.ContainsKey(res.GetType())) {
                    result = engine.ImportTypeMappers[res.GetType()](engine, res);
                } else {
                    throw new SkryptException($"No type mapper found for {res.GetType().FullName}");
                }
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
