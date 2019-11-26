using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Skrypt {
    public partial class SkryptEngine {

        public ScriptType CreateStructFromClass (Type type) {
            var newStruct = new ScriptType(type.Name, this);

            var methods = type.GetMethods();

            foreach (var method in methods) {
                newStruct.CreateProperty(method.Name, new FunctionInstance(this, (engine, self, input) => {
                    Console.WriteLine($"Method: {method.Name}");
                    Console.WriteLine($"Parameters: {string.Join(",", method.GetParameters() as object[])}");

                    var arguments = new List<object>();

                    for (int i = 0; i < input.Length; i++) {
                        if (input[0] is StringInstance) {
                            arguments.Add((input[0] as StringInstance).Value as object);
                        } else {
                            arguments.Add(input[0] as object);
                        }
                    }

                    method.Invoke(null, arguments.ToArray());

                    return null;
                }));
            }

            return newStruct;
        }

        public void AddCLRType(Type type) {
            var generatedType = CreateStructFromClass(type);

            SetGlobal(generatedType.Name, generatedType);
        }
    }
}
