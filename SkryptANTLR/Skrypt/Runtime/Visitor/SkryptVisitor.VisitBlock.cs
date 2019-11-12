using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<BaseObject> {
        public override BaseObject VisitBlock([NotNull] SkryptParser.BlockContext context) {
            if (context is IScoped scoped) {
                Console.WriteLine("Entering new scope");

                foreach (var child in _engine.CurrentEnvironment.Children) {
                    Console.WriteLine(child.Context.GetText());
                }

                _engine.CurrentEnvironment = _engine.CurrentEnvironment.Children.Find(x => x.Context == scoped);

                foreach (var variable in _engine.CurrentEnvironment.Variables) {
                    Console.WriteLine(variable);
                }
            }

            return base.VisitBlock(context);
        }
    }
}
