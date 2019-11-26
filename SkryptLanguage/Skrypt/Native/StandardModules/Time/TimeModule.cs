using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class TimeModule : BaseModule {
        public TimeModule(SkryptEngine engine) : base(engine) {}

        public static SkryptObject ExecutionTime (SkryptEngine engine, SkryptObject self) {
            return engine.CreateNumber(engine.SW.Elapsed.TotalSeconds);
        }
    }
}
