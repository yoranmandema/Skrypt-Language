using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    internal partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitIfStatement(SkryptParser.IfStatementContext context) {
            var isTrue = false;

            isTrue = Visit(context.@if().Condition).IsTrue();

            if (isTrue) {
                Visit(context.@if().stmntBlock());

                return null;
            }
            else if (context.elseif().Length > 0) {
                foreach (var stmnt in context.elseif()) {
                    isTrue = Visit(stmnt.Condition).IsTrue();

                    if (isTrue) {
                        Visit(stmnt.stmntBlock());

                        return null;
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
