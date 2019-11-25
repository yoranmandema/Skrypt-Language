using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<BaseObject> {
        public override BaseObject VisitNumberLiteral(SkryptParser.NumberLiteralContext context) {
            var value = context.number().value;
            var num = _engine.CreateNumber(value);

            LastResult = num;

            return num;
        }
    }
}
