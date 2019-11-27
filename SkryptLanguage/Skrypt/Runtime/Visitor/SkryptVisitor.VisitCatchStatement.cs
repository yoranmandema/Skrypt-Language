using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitCatchStatement(SkryptParser.CatchStatementContext context) {
            (context as IScopedContext).LexicalEnvironment.Variables[context.name().GetText()].Value = _engine.CreateString(context.error.Message);

            Visit(context.stmntBlock());

            return DefaultResult;
        }
    }
}
