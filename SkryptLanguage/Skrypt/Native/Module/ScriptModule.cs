using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class ScriptModule : SkryptModule {

        protected override void OnInitialize() {
            (GetProperty("init")?.value as FunctionInstance)?.Run();
        }

        public ScriptModule(string name, SkryptEngine engine) : base(engine) {
            Name = name;
        }
    }
}
