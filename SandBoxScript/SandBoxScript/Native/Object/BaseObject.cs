using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public class BaseObject {
        public virtual string Name { get; }

        public BaseObject StaticObject { get; set; }
        public Engine Engine { get; set; }
        public IOperation[] Operations { get; set; }
        public Dictionary<string, Member> Members = new Dictionary<string, Member>();

        public virtual void Initialise(Engine engine, params object[] args) {
            Engine = engine;
        }

        public override string ToString() {
            var str = $"{Name}\n";

            foreach (var kv in Members) {
                str += $"{kv.Key}: {kv.Value}\n";
            }

            return str;
        }
    }
}
