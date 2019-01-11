using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skrypt.ANTLR;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace Skrypt {
    public class ScriptFunction : IFunction {
        internal SkryptParser.FunctionStatementContext Context;
        internal Parameter[] Parameters;

        public ScriptFunction(SkryptParser.FunctionStatementContext block) {
            Context = block;
        }

        public BaseValue Run(Engine engine, BaseValue self, Arguments args) {
            var blockStmnt = Context.stmntBlock();
            var block = blockStmnt.block();
            var expr = blockStmnt.expression();
            var preCallValues = new Dictionary<string, BaseValue>();

            Context.Variables["self"].Value = self;

            for (int i = 0; i < Parameters.Length; i++) {
                var parameter = Parameters[i];
                var input = args[i];

                if (i < args.Values.Length) {
                    Context.Variables[parameter.Name].Value = input;
                }
                else {
                    Context.Variables[parameter.Name].Value = parameter.Default == null ? null : engine.Visitor.Visit(parameter.Default);
                }

                preCallValues[parameter.Name] = Context.Variables[parameter.Name].Value.Clone();
            }

            var returnValue = default(BaseValue);
                    
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

            foreach (var v in preCallValues) {
                if (preCallValues[v.Key].CopyOnAssignment) {
                    Context.Variables[v.Key].Value = preCallValues[v.Key];
                }
            }

            return returnValue;
        }
    }
}
