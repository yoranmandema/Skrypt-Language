using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitComputedMemberAccessExp([NotNull] SkryptParser.ComputedMemberAccessExpContext context) {
            var obj = Visit(context.expression(0));
            var index = Visit(context.expression(1));

            if (obj is StringInstance stringInstance) {
                var value = stringInstance.Get(index);

                if (value is IValue noref) value = noref.Copy();

                return value;
            }
            else if (obj is ArrayInstance arrayInstance) {
                var value = arrayInstance.Get(index);

                if (value is IValue noref) value = noref.Copy();

                return value;
            }

            _engine.ErrorHandler.FatalError(context.expression(0).Start, "Expected string or array instance.");

            return null;
        }
    }
}