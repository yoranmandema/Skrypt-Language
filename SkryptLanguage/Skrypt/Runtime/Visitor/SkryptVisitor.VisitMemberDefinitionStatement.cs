using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    internal partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitMemberDefinitionStatement(SkryptParser.MemberDefinitionStatementContext context) {
            var value = Visit(context.expression());

            if (value is IValue noref) value = noref.Copy();

            context.name().variable.Value = value;

            return DefaultResult;
        }
    }
}
