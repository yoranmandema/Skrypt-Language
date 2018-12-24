using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    internal class Scope {
        public Dictionary<string, BaseObject> Types = new Dictionary<string, BaseObject>();

        public BaseObject GetType (string name) {
            return Types[name];
        }
    }
}
