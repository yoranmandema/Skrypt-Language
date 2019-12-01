using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    internal partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitBooleanLiteral(SkryptParser.BooleanLiteralContext context) {
            return LastResult = _engine.CreateBoolean(context.boolean().value);
        }
    }
}
