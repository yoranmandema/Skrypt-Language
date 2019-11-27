using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitBlock([NotNull] SkryptParser.BlockContext context) {
            var previousEnvironment = CurrentEnvironment;

            CurrentEnvironment = CurrentEnvironment.Children.Find(x => x.Context == (context as IScopedContext));

            var result = base.VisitBlock(context);

            CurrentEnvironment = previousEnvironment;

            return result;
        }
    }
}
