using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<BaseObject> {
        public override BaseObject VisitBooleanLiteral(SkryptParser.BooleanLiteralContext context) {
            var value = context.boolean().value;
            var boolean = _engine.CreateBoolean(value);

            LastResult = boolean;

            return boolean;
        }
    }
}
