using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitMemberAccessExp(SkryptParser.MemberAccessExpContext context) {
            var obj = Visit(context.expression());
            var memberName = context.NAME().GetText();

            if (obj == null) {
                throw new NonExistingMemberException($"Tried to get member from null value.");
            }

            var property = obj.GetProperty(memberName);

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
                    _engine.ErrorHandler.FatalError(context.NAME().Symbol, $"Private property {memberName} is not accessible in the current context.");
                }
            }

            var value = property.value;

            if (value is GetPropertyInstance) {
                var newVal = (value as GetPropertyInstance).Property.Run(_engine, obj);

                value = newVal;
            }

            accessed = obj;

            LastResult = value;

            //if (value is IValue noref) value = noref.Copy();

            return value;
        }
    }
}