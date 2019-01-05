using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class ScriptType : BaseType {
        public ScriptFunction Constructor;

        public ScriptType(string name, Engine engine) : base(engine) {
            Name = name;
        }

        public override BaseInstance Construct(Arguments arguments) {
            var instance = new ScriptInstance(Engine);

            instance.GetProperties(Template);
            instance.TypeObject = this;

            var constructorResult = default(BaseValue);

            if (Constructor != null) {
                constructorResult = Constructor.Run(Engine, instance, arguments);
            }

            return instance;
        }
    }
}
