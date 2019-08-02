using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class TimeModule : BaseModule {
        public TimeModule(Engine engine) : base(engine) {}

        public static BaseObject ExecutionTime (Engine engine, BaseObject self) {
            return engine.CreateNumber(engine.SW.Elapsed.TotalSeconds);
        }
    }
}
