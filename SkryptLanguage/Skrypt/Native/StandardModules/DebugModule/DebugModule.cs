using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class DebugModule : BaseModule {
        public override string Name => "Debug";

        public DebugModule(Engine engine) : base(engine) {

        }

        public static BaseObject Assert(Engine engine, BaseObject self, Arguments arguments) {
            var condition = arguments.GetAs<BooleanInstance>(0);

            var message = arguments.Length > 1 ? arguments.GetAs<StringInstance>(1) : "Assertion failed!";

            if (!condition) {
                engine.Print(message);

                PrintCallStack(engine, self, arguments);
            }

            return condition;
        }

        public static BaseObject PrintCallStack(Engine engine, BaseObject self, Arguments arguments) {

            int i = 0;
            int count = engine.CallStack.Count();

            foreach (Call c in engine.CallStack) {
                var file = i == count - 1 ? c.callFile : c.file;
                file = file ?? c.callFile;

                engine.Print(
                    $"\tat {c.name}() in {file} ({c.line},{c.column})"
                    );

                i++;
            }

            return null;
        }
    }
}
