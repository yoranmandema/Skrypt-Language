using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class Scope {
        public Dictionary<string, Template> Templates = new Dictionary<string, Template>();
        public Dictionary<string, SkryptObject> Variables = new Dictionary<string, SkryptObject>();

        public Template GetTemplate (string name) {
            return Templates[name];
        }

        public void SetVariable (string name, SkryptObject value) {
            Variables[name] = value;
        }

        public SkryptObject GetVariable (string name) {
            if (!Variables.ContainsKey(name)) {
                throw new VariableNotFoundException($"Variable {name} not found in current context.");
            }

            return Variables[name];
        }
    }
}
