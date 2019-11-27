using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class MemoryModule : BaseModule {
        public MemoryModule(SkryptEngine engine) : base(engine) {
        }

        public static SkryptObject GetAllocatedBytes(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            var bytes = SkryptEngine.GetAllocatedBytesForCurrentThread();

            return engine.CreateNumber(bytes);
        }

        public static SkryptObject GetSizeOfObject(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            var target = arguments.GetAs<SkryptObject>(0);
            var clone = default(SkryptObject);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            var before = SkryptEngine.GetAllocatedBytesForCurrentThread();

            clone = target != null ? target.Clone() : null;

            var after = SkryptEngine.GetAllocatedBytesForCurrentThread();

            clone = null;

            return engine.CreateNumber(after - before);
        }

        public static SkryptObject GetSizeOfAction(SkryptEngine engine, SkryptObject self, Arguments arguments) {
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
