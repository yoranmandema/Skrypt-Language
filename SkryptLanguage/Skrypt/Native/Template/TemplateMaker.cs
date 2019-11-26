using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Skrypt {
    public class TemplateMaker {
        private readonly SkryptEngine _engine;

        public TemplateMaker (SkryptEngine engine) {
            _engine = engine;
        }

        public Template CreateTemplate(Type t) {
            var methods = t.GetMethods();
            var subTypes = t.GetNestedTypes();
            var template = new Template();

            if (
                !typeof(SkryptInstance).IsAssignableFrom(t) && 
                !typeof(BaseModule).IsAssignableFrom(t) && 
                !typeof(BaseType).IsAssignableFrom(t) &&
                !typeof(BaseTrait).IsAssignableFrom(t)
                ) {
                throw new InvalidTemplateTargetException("Target type must derive from BaseInstance, BaseModule, BaseType or BaseTrait.");
            }

            template.Name = System.Text.RegularExpressions.Regex.Replace(t.Name, "(Module|Instance|Type|Trait)$", "");

            foreach (var m in methods) {
                if (!m.IsStatic) continue;

                var function = default(SkryptObject);

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

            if (typeof(BaseModule).IsAssignableFrom(t)) {
                foreach (var type in subTypes) {
                    if (typeof(BaseType).IsAssignableFrom(type)) {
                        var instance = (SkryptObject)Activator.CreateInstance(type, _engine);

                        template.Members[instance.Name] = new Member(instance, type.IsNestedPrivate, null);
                    }
                }
            }

            return template;
        }
    }
}
