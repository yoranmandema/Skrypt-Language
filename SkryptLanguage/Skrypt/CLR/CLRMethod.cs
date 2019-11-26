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

        public bool HasValidArguments (Arguments arguments) {
            if (arguments.Length != parameters.Length) return false;

            var isValid = true;

            for (int i = 0; i < arguments.Length; i++) {
                var arg = arguments[i];

                if (arg is NumberInstance && typeof(double).IsAssignableFrom(parameters[i].ParameterType)) {
                    continue;
                }
                else if (arg is StringInstance && parameters[i].ParameterType == typeof(string)) {
                    continue;
                }
                else if (arg is BooleanInstance && parameters[i].ParameterType == typeof(bool)) {
                    continue;
                }
                else if (typeof(SkryptObject).IsAssignableFrom(parameters[i].ParameterType)) {
                    continue;
                }
                else if (parameters[i].ParameterType == typeof(object)) {
                    continue;
                }

                return false;
            }

            return isValid;
        }

        public object[] ConvertArguments(Arguments arguments) {
            var convertedArguments = new object[arguments.Length];

            for (int i = 0; i < arguments.Length; i++) {
                var arg = arguments[i];

                if (arg is NumberInstance numberInstance && typeof(double).IsAssignableFrom(parameters[i].ParameterType)) {
                    convertedArguments[i] = Convert.ChangeType(numberInstance.Value, parameters[i].ParameterType);
                }
                else if (arg is StringInstance stringInstance && typeof(string).IsAssignableFrom(parameters[i].ParameterType)) {
                    convertedArguments[i] = Convert.ChangeType(stringInstance.Value, parameters[i].ParameterType);
                }
                else if (arg is BooleanInstance booleanInstance && typeof(bool).IsAssignableFrom(parameters[i].ParameterType)) {
                    convertedArguments[i] = Convert.ChangeType(booleanInstance.Value, parameters[i].ParameterType);
                }
                else if (arg is SkryptObject skryptObject && typeof(SkryptObject).IsAssignableFrom(parameters[i].ParameterType)) {
                    convertedArguments[i] = skryptObject;
                }
                else if (typeof(object).IsAssignableFrom(parameters[i].ParameterType)) {
                    convertedArguments[i] = arg;
                }
            }

            return convertedArguments;
        }
    }
}
