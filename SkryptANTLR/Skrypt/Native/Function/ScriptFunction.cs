using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skrypt.ANTLR;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime;

namespace Skrypt {
    public class ScriptFunction : IFunction, IDefined {
        public string File { get; set; }
        internal IFunctionContext Context;
        internal Parameter[] Parameters;

        public ScriptFunction(IFunctionContext block) {
            Context = block;
        }

        public BaseObject Run(Engine engine, BaseObject self, Arguments args) {
            var blockStmnt = Context.StmntBlock;
            var preCallValues = new Dictionary<string, BaseObject>();

            Context.Variables["self"].Value = self;

            for (int i = 0; i < Parameters.Length; i++) {
                var parameter = Parameters[i];
                var input = args[i];

                if (input is IValue noref) input = noref.Copy();

                if (i < args.Values.Length) {
                    Context.Variables[parameter.Name].Value = input;
                }
                else {
                    Context.Variables[parameter.Name].Value = parameter.Default == null ? null : engine.Visitor.Visit(parameter.Default);
                }

                if (Context.Variables[parameter.Name].Value != null) {
                    preCallValues[parameter.Name] = Context.Variables[parameter.Name].Value.Clone();
                } else {
                    preCallValues[parameter.Name] = null;
                }
            }

            var returnValue     = default(BaseObject);
            var block           = blockStmnt.block();
            var expr            = blockStmnt.expression();
            var assignStmnt     = blockStmnt.assignStmnt();
            var returnStmnt     = blockStmnt.returnStmnt();

            if (block != null) {
                for (int i = 0; i < block.ChildCount; i++) {
                    var c = block.GetChild(i);

                    engine.Visitor.Visit(c);

                    if (Context.JumpState == JumpState.Return) {
                        Context.JumpState = JumpState.None;
                        break;
                    }
                }

                returnValue = Context.ReturnValue;
            }
            else if (expr != null) {
                returnValue = engine.Visitor.Visit(expr);
            }
            else if (assignStmnt != null) {
                engine.Visitor.Visit(assignStmnt);
            }
            else if (returnStmnt != null) {
                engine.Visitor.Visit(returnStmnt);

                returnValue = Context.ReturnValue;
            }

            foreach (var v in preCallValues) {
                if (preCallValues[v.Key] is IValue noref) {
                    Context.Variables[v.Key].Value = preCallValues[v.Key];
                }
            }

            return returnValue;
        }
    }
}
