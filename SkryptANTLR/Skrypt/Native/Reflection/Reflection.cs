using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public partial class Reflection : BaseModule {
        public Reflection(Engine engine) : base(engine) {
            CreateProperty("Value", engine.CreateString("I am reflection"));
        }

        public class Memory : BaseModule {
            public Memory(Engine engine) : base(engine) {
                CreateProperty("Value", engine.CreateString("I am memory"));
            }
        }
    }
}
