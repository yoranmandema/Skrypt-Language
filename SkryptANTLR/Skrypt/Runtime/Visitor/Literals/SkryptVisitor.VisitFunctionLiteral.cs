using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<BaseObject> {
        public override BaseObject VisitFunctionLiteral([NotNull] SkryptParser.FunctionLiteralContext context) {
            var fn = context.fnLiteral().value;

            LastResult = fn;

            return fn;
        }
    }
}
