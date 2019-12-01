using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    internal partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitIfStatement(SkryptParser.IfStatementContext context) {
            if (Visit(context.@if().Condition).IsTrue()) {
                Visit(context.@if().stmntBlock());

                return DefaultResult;
            }
            else if (context.elseif().Length > 0) {
                foreach (var stmnt in context.elseif()) {
                    if (Visit(stmnt.Condition).IsTrue()) {
                        Visit(stmnt.stmntBlock());

                        return DefaultResult;
                    }
                }
            }

            if (context.@else() != null) {
                Visit(context.@else().stmntBlock());
            }

            return DefaultResult;
        }
    }
}
