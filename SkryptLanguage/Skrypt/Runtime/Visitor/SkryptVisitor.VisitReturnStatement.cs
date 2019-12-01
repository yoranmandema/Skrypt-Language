using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    internal partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitReturnStatement(SkryptParser.ReturnStatementContext context) {
            var fnCtx = context.Statement;
            var expression = context.expression();

            if (expression != null)
                fnCtx.ReturnValue = Visit(expression);
   
            fnCtx.JumpState = JumpState.Return;

            return DefaultResult;
        }
    }
}
