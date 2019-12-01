using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    internal partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitStringLiteral(SkryptParser.StringLiteralContext context) {
            return _engine.CreateString(context.@string().value);
        }
    }
}
