using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class ScriptTrait : BaseTrait {
        public ScriptTrait(string name, SkryptEngine engine) : base(engine) {
            Name = name;
        }
    }
}
