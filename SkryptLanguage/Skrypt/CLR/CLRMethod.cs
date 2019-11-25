using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Skrypt.CLR {
    public class CLRMethod {
        public ParameterInfo[] parameters;
        public Delegate del;
        public MethodInfo methodInfo;

        public object[] ConvertArguments(Arguments arguments, out bool isValid) {
            var convertedArguments = new object[arguments.Length];
            isValid = true;

            if (arguments.Length != parameters.Length) {
                isValid = false;
                return null;
            }

            for (int i = 0; i < arguments.Length; i++) {
                var arg = arguments[i];

                if (arg is NumberInstance numberInstance && typeof(double).IsAssignableFrom(parameters[i].ParameterType)) {
                    convertedArguments[i] = Convert.ChangeType(numberInstance.Value, parameters[i].ParameterType);
                    continue;
                }
                else if (arg is StringInstance stringInstance && typeof(string).IsAssignableFrom(parameters[i].ParameterType)) {
                    convertedArguments[i] = Convert.ChangeType(stringInstance.Value, parameters[i].ParameterType);
                    continue;
                }
                else if (arg is BooleanInstance booleanInstance && typeof(bool).IsAssignableFrom(parameters[i].ParameterType)) {
                    convertedArguments[i] = Convert.ChangeType(booleanInstance.Value, parameters[i].ParameterType);
                    continue;
                }
                else if (typeof(object).IsAssignableFrom(parameters[i].ParameterType)) {
                    convertedArguments[i] = arg;
                    continue;
                }

                isValid = false;
                return null;
            }

            return convertedArguments;
        }
    }
}
