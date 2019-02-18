using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public partial class ReflectionModule : BaseModule {
        public ReflectionModule(Engine engine) : base(engine) {
            CreateProperty("Value", engine.CreateString("I am reflection"));
        }

        public class MemoryModule : BaseModule {
            public MemoryModule(Engine engine) : base(engine) {
                CreateProperty("Value", engine.CreateString("I am memory"));
            }
        }
    }
}
