using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    internal partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        void DoLoop(SkryptParser.StmntBlockContext stmntBlock, ILoopContext context, Func<bool> cond, Action callback = null) {
            var block = stmntBlock.block();
            var expression = (RuleContext)stmntBlock.expression();
            var assignStmnt = (RuleContext)stmntBlock.assignStmnt();
            var returnStmnt = (RuleContext)stmntBlock.returnStmnt();
            var continueStmnt = (RuleContext)stmntBlock.continueStmnt();
            var breakStmnt = (RuleContext)stmntBlock.breakStmnt();

            var singleLine = expression ?? assignStmnt ?? returnStmnt ?? continueStmnt ?? breakStmnt;

            if (block != null) {
                var previousEnvironment = CurrentEnvironment;

                CurrentEnvironment = CurrentEnvironment.Children.Find(x => x.Context == (block as IScopedContext));

                while (cond()) {
                    for (int i = 0; i < block.ChildCount; i++) {
                        var c = block.GetChild(i);

                        Visit(c);

                        if (context.JumpState == JumpState.Break || context.JumpState == JumpState.Continue || context.JumpState == JumpState.Return) {
                            break;
                        }
                    }

                    callback?.Invoke();

                    if (context.JumpState == JumpState.Break || context.JumpState == JumpState.Return) {
                        context.JumpState = JumpState.None;
                        break;
                    }
                    else if (context.JumpState == JumpState.Continue) {
                        context.JumpState = JumpState.None;
                        continue;
                    }
                }

                CurrentEnvironment = previousEnvironment;
            }
            else if (singleLine != null) {
                while (cond()) {
                    Visit(singleLine);

                    callback?.Invoke();

                    if (context.JumpState == JumpState.Break || context.JumpState == JumpState.Return) {
                        context.JumpState = JumpState.None;
                        break;
                    }
                    else if (context.JumpState == JumpState.Continue) {
                        context.JumpState = JumpState.None;
                        continue;
                    }
                }
            }
        }
    }
}
