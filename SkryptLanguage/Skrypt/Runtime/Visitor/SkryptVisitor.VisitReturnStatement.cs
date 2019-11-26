using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitReturnStatement(SkryptParser.ReturnStatementContext context) {
            var fnCtx = context.Statement;
            var expression = context.expression();

            if (expression != null) {
                var returnValue = Visit(expression);

                fnCtx.ReturnValue = returnValue;
            }

            fnCtx.JumpState = JumpState.Return;

            return DefaultResult;
        }
    }
}
