using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    [CLSCompliant(false)]
    internal partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitCatchStatement(SkryptParser.CatchStatementContext context) {
            var previousEnvironment = CurrentEnvironment.Parent;

            CurrentEnvironment = CurrentEnvironment.Parent.Children.Find(x => x.Context == (context as IScopedContext));

            CurrentEnvironment.Variables[context.name().GetText()].Value =
                _engine.Exception.Construct(
                    context.error.Message, 
                    DebugModule.CallStack(
                        _engine, 
                        null, 
                        Arguments.Empty
                        ).AsType<StringInstance>().Value
                    );

            Visit(context.stmntBlock());

            CurrentEnvironment = previousEnvironment;

            return DefaultResult;
        }
    }
}
