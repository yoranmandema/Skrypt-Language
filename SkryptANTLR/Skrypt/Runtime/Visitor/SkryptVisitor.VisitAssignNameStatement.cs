using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<BaseObject> {
        public override BaseObject VisitAssignNameStatement(SkryptParser.AssignNameStatementContext context) {
            var variable = CurrentEnvironment.GetVariable(context.name().GetText());

            if (variable.IsConstant) {
                _engine.ErrorHandler.FatalError(context.Start, "Constant cannot be redefined.");
            }

            var value = Visit(context.expression());

            if (value is IValue noref) value = noref.Copy();

            var op = (context.assign() as SkryptParser.AssignOperatorContext).ASSIGNOPERATOR()?.Symbol;

            if (op != null) value = EvaluateExpression(op.Text, variable.Value, value, op);

            variable.Value = value;

            return DefaultResult;
        }
    }
}
