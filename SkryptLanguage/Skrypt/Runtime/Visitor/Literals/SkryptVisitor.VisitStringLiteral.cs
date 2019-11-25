using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<BaseObject> {
        public override BaseObject VisitStringLiteral(SkryptParser.StringLiteralContext context) {
            var value = context.@string().value;
            var str = _engine.CreateString(value);

            LastResult = str;

            return str;
        }
    }
}
