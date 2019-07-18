using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<BaseObject> {
        public override BaseObject VisitMemberAccessExp(SkryptParser.MemberAccessExpContext context) {
            var obj = Visit(context.expression());
            var memberName = context.NAME().GetText();

            if (obj == null) {
                throw new NonExistingMemberException($"Tried to get member from null value.");
            }

            var property = obj.GetProperty(memberName);

            if (property.IsPrivate && property.DefinitionBlock != null) {
                var parent = context.Parent;
                var canAccess = false;

                while (parent != null) {
                    if (parent == property.DefinitionBlock) {
                        canAccess = true;
                    }

                    parent = parent.Parent;
                }

                if (!canAccess) {
                    _engine.ErrorHandler.FatalError(context.NAME().Symbol, $"Private property {memberName} is not accessible in the current context.");
                }
            }

            var value = property.Value;

            if (value is GetPropertyInstance) {
                var newVal = (value as GetPropertyInstance).Property.Run(_engine, obj);

                value = newVal;
            }

            //if (value is IValue noref) value = noref.Copy();

            accessed = obj;

            LastResult = accessed;

            return value;
        }
    }
}