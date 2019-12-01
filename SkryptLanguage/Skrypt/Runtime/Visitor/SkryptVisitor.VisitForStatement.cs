using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    internal partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitForStatement(SkryptParser.ForStatementContext context) {
            Visit(context.Instantiator);

            DoLoop(context.stmntBlock(), context,
            () => Visit(context.Condition).IsTrue(),
            () => Visit(context.Modifier));

            return DefaultResult;
        }
    }
}
