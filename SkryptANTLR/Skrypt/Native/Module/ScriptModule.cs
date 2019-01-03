using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class ScriptModule : BaseModule {
        public ScriptModule(string name, Engine engine) : base(engine) {
            Name = name;
        }
    }
}
