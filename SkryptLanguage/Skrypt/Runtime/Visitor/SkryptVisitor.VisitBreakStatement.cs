using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    internal partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitBreakStatement([NotNull] SkryptParser.BreakStatementContext context) {
            var loopCtx = context.Statement;

            if (loopCtx is SkryptParser.WhileStatementContext whileCtx)
                whileCtx.JumpState = JumpState.Break;

            return DefaultResult;
        }
    }
}
