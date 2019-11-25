using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<BaseObject> {
        public override BaseObject VisitNullLiteral([NotNull] SkryptParser.NullLiteralContext context) {
            LastResult = null;

            return null;
        }
    }
}
