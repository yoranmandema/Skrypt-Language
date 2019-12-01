using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    internal partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        //public override BaseObject VisitVector3Literal(SkryptParser.Vector3LiteralContext context) {
        //    var v = context.vector3();

        //    var x = (NumberInstance)Visit(v.X);
        //    var y = (NumberInstance)Visit(v.Y);
        //    var z = (NumberInstance)Visit(v.Z);

        //    var vec = _engine.CreateVector3(x, y, z);

        //    LastResult = vec;

        //    return vec;
        //}
    }
}
