using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace SandBoxScript {
    public class TemplateMaker {
        private readonly Engine _engine;

        public TemplateMaker (Engine engine) {
            _engine = engine;
        }

        public Template CreateTemplate(Type t) {
            var methods = t.GetMethods();
            var template = new Template();

            if (!typeof(BaseInstance).IsAssignableFrom(t) && !typeof(BaseObject).IsAssignableFrom(t))
                throw new InvalidTemplateTargetException("Target type must derive from BaseInstance or BaseObject.");

            template.Name = System.Text.RegularExpressions.Regex.Replace(t.Name, "(Object|Instance)$", "");

            foreach (var m in methods) {
                if (!m.IsStatic) continue;

                var del = (BaseDelegate)Delegate.CreateDelegate(typeof(BaseDelegate),m,false);

                if (del == null) continue; 

                var function = new FunctionObject(_engine, del);

                template.Members[m.Name] = new Member {
                    Value = function
                };
            }

            return template;
        }
    }
}
