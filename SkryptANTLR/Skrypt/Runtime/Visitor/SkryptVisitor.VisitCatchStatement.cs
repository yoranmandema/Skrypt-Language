using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<BaseObject> {
        public override BaseObject VisitCatchStatement(SkryptParser.CatchStatementContext context) {
            (context as IScoped).LexicalEnvironment.Variables[context.name().GetText()].Value = _engine.CreateString(context.error.Message);

            Visit(context.stmntBlock());

            return DefaultResult;
        }
    }
}
