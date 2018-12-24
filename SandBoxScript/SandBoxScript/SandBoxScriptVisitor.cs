using System;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;
using SandBoxScript.ANTLR;

namespace SandBoxScript {
    public class SandBoxScriptVisitor : SandBoxScriptBaseVisitor<object> {
        public delegate object Delegate(object[] p);

        public override object VisitNumericAtomExp(SandBoxScriptParser.NumericAtomExpContext context) {
            return double.Parse(context.NUMBER().GetText(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public override object VisitParenthesisExp(SandBoxScriptParser.ParenthesisExpContext context) {
            return Visit(context.expression());
        }

        public override object VisitNameExp(SandBoxScriptParser.NameExpContext context) {
            if (context.GetText() == "a") {
                return (Delegate)((object[] p) => {
                    var a = (double)(p[0]);
                    var b = (double)(p[1]);
                    var c = (double)(p[2]);

                    return a * b * c;
                });
            } else {
                throw new Exception("Name not found!");
            }
        }

        public override object VisitMulDivExp(SandBoxScriptParser.MulDivExpContext context) {
            double left = (double)Visit(context.expression(0));
            double right = (double)Visit(context.expression(1));
            double result = 0;

            if (context.ASTERISK() != null)
                result = left * right;
            if (context.SLASH() != null)
                result = left / right;

            return result;
        }

        public override object VisitAddSubExp(SandBoxScriptParser.AddSubExpContext context) {
            double left = (double)Visit(context.expression(0));
            double right = (double)Visit(context.expression(1));
            double result = 0;

            if (context.PLUS() != null)
                result = left + right;
            if (context.MINUS() != null)
                result = left - right;

            return result;
        }

        public override object VisitPowerExp(SandBoxScriptParser.PowerExpContext context) {
            double left = (double)Visit(context.expression(0));
            double right = (double)Visit(context.expression(1));
            double result = 0;

            result = Math.Pow(left, right);

            return result;
        }

        public override object VisitFunctionCallExp(SandBoxScriptParser.FunctionCallExpContext context) {
            var function = (Delegate)Visit(context.expression(0));

            var arguments = new object[context.expression().Length - 1];

            for (var i = 1; i < context.expression().Length; i++) {
                arguments[i - 1] = Visit(context.expression(i));
            }

            return function.Invoke(arguments);
        }
    }
}