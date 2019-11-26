using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitNameExp(SkryptParser.NameExpContext context) {
            var value = CurrentEnvironment.GetVariable(context.name().GetText()).Value;
            LastResult = value;

            return value;
        }
    }
}
