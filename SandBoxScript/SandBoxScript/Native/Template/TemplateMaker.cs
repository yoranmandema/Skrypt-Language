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

            foreach (var m in methods) {
                if (m.ReturnType != typeof(BaseValue)) continue;
                if (!m.IsStatic) continue;

                var parameters = m.GetParameters();

                if (parameters.Length != 3) continue;
                if (parameters[0].ParameterType != typeof(Engine)) continue;
                if (parameters[1].ParameterType != typeof(BaseValue)) continue;
                if (parameters[2].ParameterType != typeof(BaseValue[])) continue;

                var del = m.CreateDelegate(typeof(BaseDelegate), null);

                var function = new FunctionObject(_engine) {
                    Function = new DelegateFunction {
                        Function = (BaseDelegate)del
                    }
                };

                template.Members[m.Name] = new Member {
                    Value = function
                };
            }

            return template;
        }
    }
}
