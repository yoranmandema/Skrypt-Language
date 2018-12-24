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

        public void GenerateType(Type type) {
            var instanceObj = new BaseObject ();
            var staticObj = new BaseObject();

            var instance = (BaseObject)Activator.CreateInstance(type);

            var methods = type.GetRuntimeMethods();

            foreach (var m in methods) {
                if (!typeof(BaseObject).IsAssignableFrom(m.ReturnType)) continue;
                if (m.GetParameters().Length != 3) continue;
                if (m.GetParameters()[0].ParameterType != typeof(Engine)) continue;
                if (m.GetParameters()[1].ParameterType != typeof(BaseObject)) continue;
                if (m.GetParameters()[2].ParameterType != typeof(BaseObject[])) continue;

                var del = (BaseDelegate)m.CreateDelegate(typeof(BaseDelegate), instance);

                var binaryOp = (BinaryOperationAttribute)m.GetCustomAttribute(typeof(BinaryOperationAttribute));
                var unaryOp = (UnaryOperationAttribute)m.GetCustomAttribute(typeof(UnaryOperationAttribute));

                if (binaryOp != null) {
                    var operation = new BinaryOperation {
                        Name = binaryOp.Name,
                        LeftType = binaryOp.LeftType,
                        RightType = binaryOp.RightType,
                        Function = new DelegateFunction {
                            Function = del
                        }
                    };

                    instanceObj.Operations.Add(operation);
                }
                else if (unaryOp != null) {
                    var operation = new UnaryOperation {
                        Name = binaryOp.Name,
                        Type = binaryOp.LeftType,
                        Function = new DelegateFunction {
                            Function = del
                        }
                    };

                    instanceObj.Operations.Add(operation);
                }
                else {
                    var function = new FunctionObject {
                        Function = new DelegateFunction {
                            Function = del
                        }
                    };

                    if (m.GetCustomAttributes(typeof(StaticAttribute)).Any()) {
                        staticObj.Members[m.Name] = new Member {
                            Value = function
                        };
                    }
                    else {
                        instanceObj.Members[m.Name] = new Member {
                            Value = function
                        };
                    }
                }
            }

            Scope.Variables[instance.Name]  = staticObj;
            Scope.Types[instance.Name]      = instanceObj;
        }

        public BaseObject Run (string code) {
            GenerateType(typeof(NumberObject));

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
