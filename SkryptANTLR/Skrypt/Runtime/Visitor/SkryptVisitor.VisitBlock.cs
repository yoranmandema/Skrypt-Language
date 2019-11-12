using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<BaseObject> {
        public override BaseObject VisitBlock([NotNull] SkryptParser.BlockContext context) {
            if (context is IScoped scoped) {
                _engine.CurrentEnvironment = _engine.CurrentEnvironment.Children.Find(x => x.Context == scoped);
            }

            return base.VisitBlock(context);
        }
    }
}
