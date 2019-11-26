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
        internal LexicalEnvironment BaseEnvironment;

        public ScriptFunction(IFunctionContext block) {
            Context = block;
        }


        public SkryptObject Run(SkryptEngine engine, SkryptObject self, Arguments args) {
            var blockStmnt = Context.StmntBlock;
            var visitor = new SkryptVisitor(engine);
            var lexicalEnvironment = LexicalEnvironment.MakeCopy(Context.LexicalEnvironment);

            if (BaseEnvironment != null) lexicalEnvironment.Parent = BaseEnvironment;
            lexicalEnvironment.Variables["self"].Value = self;

            for (int i = 0; i < Parameters.Length; i++) {
                var parameter = Parameters[i];
                var input = args[i];

                if (input is IValue noref) input = noref.Copy();

                if (i < args.Values.Length) {
                    lexicalEnvironment.Variables[parameter.Name].Value = input;
                }
                else {
                    lexicalEnvironment.Variables[parameter.Name].Value = parameter.Default == null ? null : engine.Visitor.Visit(parameter.Default);
                }
            }

            var returnValue     = default(SkryptObject);
            var block           = blockStmnt.block();
            var expr            = blockStmnt.expression();
            var assignStmnt     = blockStmnt.assignStmnt();
            var returnStmnt     = blockStmnt.returnStmnt();

            visitor.CurrentEnvironment = lexicalEnvironment;

            if (block != null) {

                var prev = visitor.CurrentEnvironment;

                visitor.CurrentEnvironment = visitor.CurrentEnvironment.Children.Find(x => x.Context == block);

                for (int i = 0; i < block.ChildCount; i++) {
                    var c = block.GetChild(i);

                    visitor.Visit(c);

                    if (Context.JumpState == JumpState.Return) {
                        Context.JumpState = JumpState.None;
                        break;
                    }
                }

                returnValue = Context.ReturnValue;
            }
            else if (expr != null) {
                returnValue = visitor.Visit(expr);
            }
            else if (assignStmnt != null) {
                visitor.Visit(assignStmnt);
            }
            else if (returnStmnt != null) {
                visitor.Visit(returnStmnt);

                returnValue = Context.ReturnValue;
            }

            if (returnValue is FunctionInstance functionInstance && functionInstance.Function is ScriptFunction scriptFunction) {
                var checkParent = scriptFunction.Context.Context;
                var isDefinedInCurrentFunction = false;

                while (checkParent.Parent != null) {
                    if (checkParent == Context.Context) {
                        isDefinedInCurrentFunction = true;
                        break;
                    }

                    checkParent = checkParent.Parent;
                }

                if (isDefinedInCurrentFunction) 
                    scriptFunction.BaseEnvironment = visitor.CurrentEnvironment;
            }
            
           // BaseEnvironment = null;

            return returnValue;
        }
    }
}
