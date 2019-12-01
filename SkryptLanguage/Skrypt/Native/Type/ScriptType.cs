using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class ScriptType : SkryptType, IDefined {
        public string File { get; set; }
        public ScriptFunction Constructor;

        public ScriptType(string name, SkryptEngine engine) : base(engine) {
            Name = name;
        }

        public override SkryptInstance Construct(Arguments arguments) {
            var instance = new ScriptInstance(Engine);

            instance.GetProperties(Template);
            instance.TypeObject = this;

            Constructor?.Run(Engine, instance, arguments);

            return instance;
        }
    }
}
