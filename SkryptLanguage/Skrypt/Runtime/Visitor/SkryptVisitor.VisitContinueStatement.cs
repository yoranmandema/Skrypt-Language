using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    internal partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitContinueStatement([NotNull] SkryptParser.ContinueStatementContext context) {
            var loopCtx = context.Statement;

            if (loopCtx is SkryptParser.WhileStatementContext whileCtx)
                whileCtx.JumpState = JumpState.Continue;

            return DefaultResult;
        }
    }
}
