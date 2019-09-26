using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<BaseObject> {
        public override BaseObject VisitTryStatement(SkryptParser.TryStatementContext context) {

            try {
                Visit(context.stmntBlock());
            } catch (Exception e) {
                var catchStmnt = context.catchStmt();
                catchStmnt.error = e;

                Visit(catchStmnt);
            }

            return DefaultResult;
        }
    }
}
