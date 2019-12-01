using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    internal partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitWhileStatement(SkryptParser.WhileStatementContext context) {
            DoLoop(context.stmntBlock(), context, () => {
                return Visit(context.Condition).IsTrue();
            });

            return DefaultResult;
        }
    }
}
