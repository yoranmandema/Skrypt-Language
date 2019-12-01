using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    internal partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitMemberAccessExp(SkryptParser.MemberAccessExpContext context) {
            var target = Visit(context.expression());
            var memberName = context.NAME().GetText();

            if (target == null) {
                throw new NonExistingMemberException($"Tried to get member from null value.");
            }

            if (!target.GetPropertyInContext(memberName, context, out Member property))
                _engine.ErrorHandler.FatalError(context.NAME().Symbol, $"Private property {memberName} is not accessible in the current context.");

            var value = property.value;

            if (value is GetPropertyInstance)
                value = (value as GetPropertyInstance)?.Property.Run(_engine, target);

            accessed = target;

            LastResult = value;

            if (value is IValue noref) value = noref.Copy();

            return value;
        }
    }
}