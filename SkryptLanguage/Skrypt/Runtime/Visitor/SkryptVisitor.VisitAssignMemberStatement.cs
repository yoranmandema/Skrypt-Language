using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<BaseObject> {
        public override BaseObject VisitAssignMemberStatement(SkryptParser.AssignMemberStatementContext context) {
            var target = Visit(context.memberAccess().expression());
            var memberName = context.memberAccess().NAME().GetText();

            var property = target.GetProperty(memberName);

            if (property.isConstant) _engine.ErrorHandler.FatalError(context.Start, "Constant member cannot be redefined.");

            if (property.isPrivate && property.definitionBlock != null) {
                var parent = context.Parent;
                var canAccess = false;

                while (parent != null) {
                    if (parent == property.definitionBlock) {
                        canAccess = true;
                    }

                    parent = parent.Parent;
                }

                if (!canAccess) {
                    _engine.ErrorHandler.FatalError(context.memberAccess().Start, $"Private property {memberName} is not accessible in the current context.");
                }
            }

            var value = Visit(context.expression());

            if (value is IValue noref) value = noref.Copy();

            target.SetProperty(memberName, value);

            return DefaultResult;
        }
    }
}
