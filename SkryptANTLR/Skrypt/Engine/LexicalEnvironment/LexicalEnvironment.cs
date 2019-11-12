using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class LexicalEnvironment {
        public Dictionary<string, Variable> Variables { get; set; } = new Dictionary<string, Variable>();
        public RuleContext Context { get; internal set; }
        public LexicalEnvironment Parent { get; set; }
        public List<LexicalEnvironment> Children { get; set; } = new List<LexicalEnvironment>();

        public void AddVariable (Variable variable) {
            Variables[variable.Name] = variable;
        }

        public Variable GetVariable (string name) {
            if (Variables.ContainsKey(name)) {
                return Variables[name];
            } else if (Parent != null)  {
                return Parent.GetVariable(name);
            } else {
                throw new VariableNotFoundException($"Variable {name} not found in current context.");
            }
        }

        public void AddChild (LexicalEnvironment child) {
            child.Parent = this;
            Children.Add(child);
        }

        public static LexicalEnvironment MakeCopy (LexicalEnvironment lexicalEnvironment) {
            var newEnvironment = new LexicalEnvironment {
                Context = lexicalEnvironment.Context,
                Parent = lexicalEnvironment.Parent
            };

            foreach (var kv in lexicalEnvironment.Variables) {
                var variable = new Variable(kv.Key);

                if (kv.Value.Value is IValue valueType) {
                    variable.Value = valueType.Copy();
                } else {
                    variable.Value = kv.Value.Value;
                }

                newEnvironment.Variables[kv.Key] = variable;
            }

            newEnvironment.Children = CopyChildrenRecursively(lexicalEnvironment);

            return newEnvironment;
        }

        public static List<LexicalEnvironment> CopyChildrenRecursively (LexicalEnvironment lexicalEnvironment) {
            if (!lexicalEnvironment.Children.Any()) {
                return null;
            }
            
            var newChildren = new List<LexicalEnvironment>();

            foreach (var child in lexicalEnvironment.Children) {
                var newChild = MakeCopy(child);
                newChild.Children = CopyChildrenRecursively(child);

                newChildren.Add(newChild);
            }

            return newChildren;
        }
    }
}
