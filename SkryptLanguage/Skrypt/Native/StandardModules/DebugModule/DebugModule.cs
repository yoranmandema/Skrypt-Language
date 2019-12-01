using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class DebugModule : SkryptModule {
        public override string Name => "Debug";

        public DebugModule(SkryptEngine engine) : base(engine) {

        }

        public static SkryptObject Assert(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            var condition = arguments.GetAs<BooleanInstance>(0);

            var message = arguments.Length > 1 ? arguments.GetAs<StringInstance>(1) : "Assertion failed!";

            if (!condition) {
                Console.WriteLine(message);

                Console.WriteLine(CallStack(engine, self, arguments));
            }

            return condition;
        }

        public static SkryptObject CallStack(SkryptEngine engine, SkryptObject self, Arguments arguments) {

            int i = 0;
            int count = engine.CallStack.Count();
            string str = "";

            foreach (Call c in engine.CallStack) {
                var file = i == count - 1 ? c.callFile : c.file;
                file = file ?? c.callFile;

                str += $"\tat {c.name}() in {file} ({c.line},{c.column})\n";

                i++;
            }

            return engine.CreateString(str);
        }
    }
}
