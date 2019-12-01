using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    internal partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitConditionalExp(SkryptParser.ConditionalExpContext context) {
            SkryptObject value;

            if (Visit(context.expression(0)).IsTrue()) {
                value = Visit(context.expression(1));
            } else {
                value = Visit(context.expression(2));
            }

            if (value is IValue noref) value = noref.Copy();

            LastResult = value;

            return value;
        }
    }
}
