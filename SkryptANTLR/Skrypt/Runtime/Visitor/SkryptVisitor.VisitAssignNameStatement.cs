using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<BaseObject> {
        public override BaseObject VisitAssignNameStatement(SkryptParser.AssignNameStatementContext context) {
            if (context.name().variable.IsConstant) {
                _engine.ErrorHandler.FatalError(context.Start, "Constant cannot be redefined.");
            }

            var value = Visit(context.expression());

            if (value is IValue noref) value = noref.Copy();

            CurrentEnvironment.Variables[context.name().GetText()].Value = value;

            return DefaultResult;
        }
    }
}
