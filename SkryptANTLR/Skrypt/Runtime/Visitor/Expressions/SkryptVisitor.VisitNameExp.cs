using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<BaseObject> {
        public override BaseObject VisitNameExp(SkryptParser.NameExpContext context) {
            var value = context.name().variable.Value;

            LastResult = value;

            return context.name().variable.Value;
        }
    }
}
