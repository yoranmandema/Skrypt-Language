using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    internal partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitForStatement(SkryptParser.ForStatementContext context) {
            Visit(context.Instantiator);

            DoLoop(context.stmntBlock(), context,
            () => {
                var result = Visit(context.Condition).IsTrue();

                return result;
            },
            () => {
                Visit(context.Modifier);
            });

            return DefaultResult;
        }
    }
}
