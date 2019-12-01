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

        public SkryptObject Run(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            SkryptObject result = null;
            bool validArguments = false;

            foreach (var function in Functions) {
                var isValid = function.HasValidArguments(arguments);

                if (isValid) {
                    var args = function.ConvertArguments(arguments);

                    validArguments = isValid;

                    var res = function.del.DynamicInvoke(args);

                    if (res != null) {
                        if (engine.ImportTypeMappers.ContainsKey(res.GetType())) {
                            result = engine.ImportTypeMappers[res.GetType()](engine, res);
                        }
                        else {
                            throw new SkryptException($"No type mapper found for {res.GetType().FullName}");
                        }
                    }

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
