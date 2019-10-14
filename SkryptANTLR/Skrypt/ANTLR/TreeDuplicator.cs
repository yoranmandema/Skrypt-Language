using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System.Reflection;

namespace Skrypt.ANTLR {
    public class TreeDuplicator : SkryptBaseVisitor<BaseObject> {

        public static ParserRuleContext DuplicateTree (ParserRuleContext context) {
            var newContext = (ParserRuleContext)Activator.CreateInstance(context.GetType(), context);
            newContext.CopyFrom(context);

            DuplicateChildren(context, newContext);

            Console.WriteLine(newContext.GetText());

            return newContext;
        }

        private static ParserRuleContext DuplicateContext(ParserRuleContext context) {
            var constructor = context.GetType().GetConstructor(new Type[] {typeof(SkryptParser.ExpressionContext)});
            ParserRuleContext newContext = null;

            Console.WriteLine(constructor);

            if (constructor != null) {
                newContext = constructor.Invoke(new object[] { context as SkryptParser.ExpressionContext }) as ParserRuleContext; 
            }
            else {
                newContext = (ParserRuleContext)Activator.CreateInstance(context.GetType(), context, context.invokingState);
            }

            newContext.CopyFrom(context);

            return newContext;
        }

        private static ITerminalNode DuplicateTerminalNode(ITerminalNode context) {
            var newTerminalNode = (ITerminalNode)Activator.CreateInstance(context.GetType(), context.Symbol);

            return newTerminalNode;
        }

        public static void DuplicateChildren (ParserRuleContext originalBranch, ParserRuleContext branch) {     
            
            for (int i = 0; i < originalBranch.ChildCount; i++) {
                //Console.WriteLine(originalBranch.GetChild(i).GetType());
                var child = originalBranch.GetChild(i);

                if (child is ITerminalNode) {
                    var copy = DuplicateTerminalNode(child as ITerminalNode);
                    branch.AddChild(copy);
                } else if (child is RuleContext) {
                    var copy = DuplicateContext(child as ParserRuleContext);
                    branch.AddChild(copy);

                    DuplicateChildren(child as ParserRuleContext, branch.GetChild(i) as ParserRuleContext);
                }
            }
        }
    }
}
