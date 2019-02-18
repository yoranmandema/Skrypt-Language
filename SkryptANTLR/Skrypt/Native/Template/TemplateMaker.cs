using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Skrypt {
    public class TemplateMaker {
        private readonly Engine _engine;

        public TemplateMaker (Engine engine) {
            _engine = engine;
        }

        public Template CreateTemplate(Type t) {
            var methods = t.GetMethods();
            var types = t.GetNestedTypes();
            var template = new Template();

            if (!typeof(BaseInstance).IsAssignableFrom(t) && !typeof(BaseModule).IsAssignableFrom(t) && !typeof(BaseType).IsAssignableFrom(t)) {
                throw new InvalidTemplateTargetException("Target type must derive from BaseInstance, BaseModule or BaseType.");
            }

            template.Name = System.Text.RegularExpressions.Regex.Replace(t.Name, "(Module|Instance|Type)$", "");

            foreach (var m in methods) {
                if (!m.IsStatic) continue;

                var function = default(BaseObject);

                var methodDelegate = (MethodDelegate)Delegate.CreateDelegate(typeof(MethodDelegate),m,false);

                if (methodDelegate != null) {
                    function = new FunctionInstance(_engine, methodDelegate);
                }

                var getPropertyDelegate = (GetPropertyDelegate)Delegate.CreateDelegate(typeof(GetPropertyDelegate), m, false);

                if (getPropertyDelegate != null) {
                    function = new GetPropertyInstance(_engine, getPropertyDelegate);
                }

                template.Members[m.Name] = new Member(function, m.IsPrivate, null);
            }

            foreach (var type in types) {
                var instance = (BaseModule)Activator.CreateInstance(type, _engine);

                template.Members[instance.Name] = new Member(instance, type.IsNestedPrivate, null);
            }

            return template;
        }
    }
}
