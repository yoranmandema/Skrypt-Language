using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<BaseObject> {
        public override BaseObject VisitWhileStatement(SkryptParser.WhileStatementContext context) {
            DoLoop(context.stmntBlock(), context, () => {
                return Visit(context.Condition).IsTrue();
            });

            return DefaultResult;
        }
    }
}
