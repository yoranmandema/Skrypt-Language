using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    internal partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        //public override BaseObject VisitVector2Literal(SkryptParser.Vector2LiteralContext context) {
        //    var v = context.vector2();

        //    var x = (NumberInstance)Visit(v.X);
        //    var y = (NumberInstance)Visit(v.Y);

        //    var vec = _engine.CreateVector2(x, y);

        //    LastResult = vec;

        //    return vec;
        //}
    }
}
