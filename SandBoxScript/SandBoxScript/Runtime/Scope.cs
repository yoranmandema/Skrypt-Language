using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public class Scope {
        public Dictionary<string, Template> Templates = new Dictionary<string, Template>();
        public Dictionary<string, BaseObject> Variables = new Dictionary<string, BaseObject>();

        public Template GetTemplate (string name) {
            return Templates[name];
        }
    }
}
