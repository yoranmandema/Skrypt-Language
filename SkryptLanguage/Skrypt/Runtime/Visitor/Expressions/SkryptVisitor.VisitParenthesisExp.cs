using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitParenthesisExp(SkryptParser.ParenthesisExpContext context) {
            return Visit(context.expression());
        }
    }
}
