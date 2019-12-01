using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    internal partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitNumberLiteral(SkryptParser.NumberLiteralContext context) {
            return LastResult = _engine.CreateNumber(context.number().value);
        }
    }
}
