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
        private readonly MethodInfo _methodInfo;
        private readonly SkryptEngine _engine;

        public CLRMethod (SkryptEngine e, MethodInfo methodInfo) {
            _engine = e;
            methodInfo = _methodInfo;
        }

        public bool HasValidArguments (Arguments arguments) {
            if (arguments.Length != parameters.Length) return false;

            var isValid = true;

            for (int i = 0; i < arguments.Length; i++) {
                var arg = arguments[i];

                if (_engine.ExportTypeMappers.ContainsKey(arg.GetType())) {
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

                if (_engine.ExportTypeMappers.ContainsKey(arg.GetType())) {
                    convertedArguments[i] = _engine.ExportTypeMappers[arg.GetType()](arg);
                }
                else if (parameters[i].ParameterType == typeof(object)) {
                    convertedArguments[i] = arg;
                }
            }

            return convertedArguments;
        }
    }
}
