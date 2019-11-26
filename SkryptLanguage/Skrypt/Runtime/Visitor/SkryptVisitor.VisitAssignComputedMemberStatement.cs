using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitAssignComputedMemberStatement([NotNull] SkryptParser.AssignComputedMemberStatementContext context) {
            var lhs = context.memberAccessComp();

            var obj = Visit(lhs.expression(0));
            var index = Visit(lhs.expression(1));
            var value = Visit(context.expression());

            if (value is IValue noref) value = noref.Copy();

            if (obj is ArrayInstance arrayInstance) {
                return arrayInstance.Set(index, value);
            }

            _engine.ErrorHandler.FatalError(lhs.expression(0).Start, "Expected array instance.");

            return null;
        }
    }
}