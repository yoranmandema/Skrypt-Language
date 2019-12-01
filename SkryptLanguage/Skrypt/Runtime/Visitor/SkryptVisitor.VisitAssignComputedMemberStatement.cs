using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    internal partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitAssignComputedMemberStatement([NotNull] SkryptParser.AssignComputedMemberStatementContext context) {
            var lhs = context.memberAccessComp();

            var value = Visit(context.expression());

            if (value is IValue noref) value = noref.Copy();

            if (Visit(lhs.expression(0)) is ArrayInstance arrayInstance) {
                return arrayInstance.Set(Visit(lhs.expression(1)), value);
            }

            _engine.ErrorHandler.FatalError(lhs.expression(0).Start, "Expected array instance.");

            return null;
        }
    }
}