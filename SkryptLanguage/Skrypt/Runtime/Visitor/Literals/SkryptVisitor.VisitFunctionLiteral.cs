using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    internal partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitFunctionLiteral([NotNull] SkryptParser.FunctionLiteralContext context) {
            return LastResult = context.fnLiteral().value;
        }
    }
}
