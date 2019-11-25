using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Skrypt.CLR {
    public class CLRFunction {
        public ParameterInfo[] parameters;
        public Delegate del;

        public object[] ConvertArguments (Arguments arguments, out bool isValid) {
            var convertedArguments = new object[arguments.Length];
            isValid = true;

            if (arguments.Length != parameters.Length) {
                isValid = false;
                return null;
            }

            for (int i = 0; i < arguments.Length; i++) {
                var arg = arguments[i];

                if (arg is NumberInstance numberInstance) {
                    if (parameters[i].ParameterType.IsAssignableFrom(typeof(double))) {
                        convertedArguments[i] = Convert.ChangeType(numberInstance.Value, parameters[i].ParameterType);
                    }
                }
            }

            return convertedArguments;
        }
    }
}
