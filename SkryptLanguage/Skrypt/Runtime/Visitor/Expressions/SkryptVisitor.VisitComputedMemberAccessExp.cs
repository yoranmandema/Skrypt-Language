using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    internal partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitComputedMemberAccessExp([NotNull] SkryptParser.ComputedMemberAccessExpContext context) {
            var obj = Visit(context.expression(0));
            var index = Visit(context.expression(1));
            SkryptObject value = null;

            if (!(obj is StringInstance) && !(obj is ArrayInstance))
                _engine.ErrorHandler.FatalError(context.expression(0).Start, "Expected string or array instance.");

            if (obj is StringInstance stringInstance) {
                value = stringInstance.Get(index);
            }
            else if (obj is ArrayInstance arrayInstance) {
                value = arrayInstance.Get(index);
            }

            if (value is IValue noref) value = noref.Copy();

            return value;
        }
    }
}