using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitCatchStatement(SkryptParser.CatchStatementContext context) {
            CurrentEnvironment = CurrentEnvironment.Parent;

            var stackTrace = DebugModule.CallStack(_engine, null, Arguments.Empty);
            var previousEnvironment = CurrentEnvironment;
            var block = context.stmntBlock();

            CurrentEnvironment = CurrentEnvironment.Children.Find(x => x.Context == (context as IScopedContext));

            CurrentEnvironment.Variables[context.name().GetText()].Value =
                _engine.Exception.Construct(context.error.Message, stackTrace.AsType<StringInstance>().Value);

            Visit(block);

            CurrentEnvironment = previousEnvironment;

            return DefaultResult;
        }
    }
}
