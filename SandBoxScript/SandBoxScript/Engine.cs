using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4;
using SandBoxScript.ANTLR;
using System.Reflection;
using SandBoxScript.MethodInfoExtensions;

namespace SandBoxScript {
    public class Engine {
        internal Scope Scope = new Scope();

        public BaseObject GenerateType(Type type) {
            var obj = new BaseObject {
                Engine = this
            };

            var methods = type.GetRuntimeMethods();

            var instance = Activator.CreateInstance(type, null);

            foreach (var m in methods) {
                if (!typeof(BaseObject).IsAssignableFrom(m.ReturnType)) continue;

                
            }

            return null;
        }

        public void CreateBaseTypes () {
            Scope.Types["Number"] = GenerateType(typeof(NumberObject));
        }

        public BaseObject Run (string code) {
            CreateBaseTypes();

            var inputStream         = new AntlrInputStream(code);
            var sandBoxScriptLexer  = new SandBoxScriptLexer(inputStream);
            var commonTokenStream   = new CommonTokenStream(sandBoxScriptLexer);
            var sandBoxScriptParser = new SandBoxScriptParser(commonTokenStream);

            var expressionContext = sandBoxScriptParser.expression();
            var visitor = new SandBoxScriptVisitor(this);

            return visitor.Visit(expressionContext);
        }

        public T Create<T> (params object[] args) where T : BaseObject {
            var newObject = (BaseObject)Activator.CreateInstance(typeof(T), args);
            newObject.Engine = this;

            var staticObject = Scope.GetType(newObject.Name);

            newObject.Members = new Dictionary<string, Member>(staticObject.Members);
            newObject.Operations = staticObject.Operations;
            newObject.StaticObject = staticObject;

            return (T)newObject;
        }
    }
}
