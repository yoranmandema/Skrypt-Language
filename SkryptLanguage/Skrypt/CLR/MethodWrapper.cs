using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Skrypt.CLR {
    public class MethodWrapper {
        public static CLRFunction CreateCLRFunction (MethodInfo methodInfo) {
            var newCLRFunction = new CLRFunction() {
                parameters = methodInfo.GetParameters()
            };

            return newCLRFunction;
        }
    }
}
