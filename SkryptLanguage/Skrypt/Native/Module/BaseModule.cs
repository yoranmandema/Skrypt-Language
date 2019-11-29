using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class BaseModule : SkryptObject {
        public BaseModule(SkryptEngine engine) : base(engine) {
            var template = engine.TemplateMaker.CreateTemplate(this.GetType());

            GetProperties(template.Members);

            foreach (var member in Members) {
                if (member.Value.value is FunctionInstance function) {
                    function.Self = this;
                }
            }

            Name = template.Name;
        }
    }
}
