using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript.ANTLR {
    public partial class SandBoxScriptBaseVisitor<Result> : AbstractParseTreeVisitor<Result>, ISandBoxScriptVisitor<Result> {
        //protected override bool ShouldVisitNextChild([NotNull] IRuleNode node, Result currentResult) {
        //    return true;
        //}

        //public override Result VisitChildren(IRuleNode node) {
        //    Result result = DefaultResult;
        //    int n = node.ChildCount;
        //    for (int i = 0; i < n; i++) {
        //        if (!ShouldVisitNextChild(node, result)) {
        //            break;
        //        }

        //        IParseTree c = node.GetChild(i);
        //        Console.WriteLine("Child: " + c.Parent.GetType());

        //        if (c is SandBoxScriptParser.ReturnStatementContext) break; 

        //        Result childResult = c.Accept(this);
        //        result = AggregateResult(result, childResult);
        //    }

        //    return result;
        //}
    }
}