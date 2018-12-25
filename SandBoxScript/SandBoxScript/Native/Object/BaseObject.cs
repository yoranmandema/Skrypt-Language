using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public class BaseObject : BaseValue {
        public BaseObject(Engine engine) : base(engine) {
            var template = engine.templateMaker.CreateTemplate(this.GetType());

            GetProperties(template.Members);
        }
    }
}
