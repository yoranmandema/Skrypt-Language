using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    internal partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitArrayLiteral([NotNull] SkryptParser.ArrayLiteralContext context) {
            var a = context.array();

            var expressions = a.expressionGroup().expression();
            var values = new SkryptObject[expressions.Length];

            for (int i = 0; i < values.Length; i++) {
                values[i] = Visit(expressions[i]);
            }

            var array = _engine.CreateArray(values);

            LastResult = array;

            return array;
        }
    }
}
