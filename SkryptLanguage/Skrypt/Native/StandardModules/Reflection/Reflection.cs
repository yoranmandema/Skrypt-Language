using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public partial class ReflectionModule : BaseModule {
        public ReflectionModule(SkryptEngine engine) : base(engine) {
        }

        public class MemoryModule : BaseModule {
            public MemoryModule(SkryptEngine engine) : base(engine) {
            }

            public static SkryptObject GetSizeOfObject (SkryptEngine engine, SkryptObject self, Arguments arguments) {
                var target = arguments.GetAs<SkryptObject>(0);
                var clone = default(SkryptObject);

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

                var before = GC.GetTotalMemory(true);

                clone = target != null ? target.Clone() : null;

                var after = GC.GetTotalMemory(true);
                clone = null;

                return engine.CreateNumber(after - before);
            }

            public static SkryptObject GetSizeOfAction (SkryptEngine engine, SkryptObject self, Arguments arguments) {
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
