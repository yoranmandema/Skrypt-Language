using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class SkryptModule : SkryptObject {
        public SkryptModule(SkryptEngine engine) : base(engine) {
            var template = engine.TemplateMaker.CreateTemplate(this.GetType());

            GetProperties(template.Members);

            Name = template.Name;
        }
    }
}
