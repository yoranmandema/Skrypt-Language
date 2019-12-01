using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    internal partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitStringLiteral(SkryptParser.StringLiteralContext context) {
            var value = context.@string().value;
            var str = _engine.CreateString(value);

            LastResult = str;

            return str;
        }
    }
}
