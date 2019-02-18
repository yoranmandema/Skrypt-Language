using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public partial class ReflectionModule : BaseModule {
        public ReflectionModule(Engine engine) : base(engine) {
        }

        public class MemoryModule : BaseModule {
            public MemoryModule(Engine engine) : base(engine) {
            }

            public static BaseObject GetSizeOfObject (Engine engine, BaseObject self, Arguments arguments) {
                var target = arguments.GetAs<BaseObject>(0);
                var clone = default(BaseObject);

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                var before = GC.GetTotalMemory(true);

                clone = target != null ? target.Clone() : null;

                var after = GC.GetTotalMemory(true);
                clone = null;

                return engine.CreateNumber(after - before);
            }

            public static BaseObject GetSizeOfAction (Engine engine, BaseObject self, Arguments arguments) {
                var target = arguments.GetAs<FunctionInstance>(0);

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                var before = GC.GetTotalMemory(true);

                target.Run();

                var after = GC.GetTotalMemory(true);

                return engine.CreateNumber(after - before);
            }
        }
    }
}
