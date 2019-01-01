using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SandBoxScript.ANTLR;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace SandBoxScript {
    public class ScriptFunction : IFunction {
        internal SandBoxScriptParser.FunctionStatementContext Context;
        internal Parameter[] Parameters;

        public ScriptFunction(SandBoxScriptParser.FunctionStatementContext block) {
            Context = block;
        }

        public BaseValue Run(Engine engine, BaseValue self, Arguments args) {
            var blockStmnt = Context.stmntBlock();
            var block = blockStmnt.block();
            var expr = blockStmnt.expression();
            var preCallValues = new Dictionary<string, BaseValue>();

            for (int i = 0; i < Parameters.Length; i++) {
                var parameter = Parameters[i];
                var input = args[i];

                if (i < args.Values.Length) {
                    Context.ParameterVariables[parameter.Name].Value = input;
                }
                else {
                    Context.ParameterVariables[parameter.Name].Value = parameter.Default == null ? null : engine.Visitor.Visit(parameter.Default);
                }

                preCallValues[parameter.Name] = Context.ParameterVariables[parameter.Name].Value.Clone();
            }

            var returnValue = default(BaseValue);
                    
            if (block != null) {
                engine.Visitor.Visit(block);

                returnValue = Context.ReturnValue;
            }
            else if (expr != null) {
                returnValue = engine.Visitor.Visit(expr);
            }

            foreach (var v in Context.ParameterVariables) {
                if (preCallValues[v.Key].CopyOnAssignment) {
                    Context.ParameterVariables[v.Key].Value = preCallValues[v.Key];
                }
            }

            return returnValue;
        }
    }
}
