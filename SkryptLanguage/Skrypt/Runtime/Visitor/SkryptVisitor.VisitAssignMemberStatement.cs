using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    internal partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitAssignMemberStatement(SkryptParser.AssignMemberStatementContext context) {
            var target = Visit(context.memberAccess().expression());
            var memberName = context.memberAccess().NAME().GetText();

            if (!target.GetPropertyInContext(memberName, context, out Member property))
                _engine.ErrorHandler.FatalError(context.memberAccess().Start, $"Private property {memberName} is not accessible in the current context.");

            if (property.isConstant) _engine.ErrorHandler.FatalError(context.Start, "Constant member cannot be redefined.");

            var value = Visit(context.expression());

            if (value is IValue noref) value = noref.Copy();

            target.SetProperty(memberName, value);

            return DefaultResult;
        }
    }
}
