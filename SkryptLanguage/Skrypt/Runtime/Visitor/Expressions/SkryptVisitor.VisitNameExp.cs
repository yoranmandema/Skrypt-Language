using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    internal partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitNameExp(SkryptParser.NameExpContext context) {
            return LastResult = CurrentEnvironment.GetVariable(context.name().GetText()).Value;
        }
    }
}
