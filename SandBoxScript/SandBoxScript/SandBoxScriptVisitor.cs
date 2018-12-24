using System;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;
using SandBoxScript.ANTLR;

namespace SandBoxScript {
    public class SandBoxScriptVisitor : SandBoxScriptBaseVisitor<double> {
        public override double VisitNumericAtomExp(SandBoxScriptParser.NumericAtomExpContext context) {
            return double.Parse(context.NUMBER().GetText(), System.Globalization.CultureInfo.InvariantCulture);
        }

        public override double VisitParenthesisExp(SandBoxScriptParser.ParenthesisExpContext context) {
            return Visit(context.expression());
        }

        public override double VisitMulDivExp(SandBoxScriptParser.MulDivExpContext context) {
            double left = Visit(context.expression(0));
            double right = Visit(context.expression(1));
            double result = 0;

            if (context.ASTERISK() != null)
                result = left * right;
            if (context.SLASH() != null)
                result = left / right;

            return result;
        }

        public override double VisitAddSubExp(SandBoxScriptParser.AddSubExpContext context) {
            double left = Visit(context.expression(0));
            double right = Visit(context.expression(1));
            double result = 0;

            if (context.PLUS() != null)
                result = left + right;
            if (context.MINUS() != null)
                result = left - right;

            return result;
        }

        public override double VisitPowerExp(SandBoxScriptParser.PowerExpContext context) {
            double left = Visit(context.expression(0));
            double right = Visit(context.expression(1));
            double result = 0;

            result = Math.Pow(left, right);

            return result;
        }

        public override double VisitFunctionExp(SandBoxScriptParser.FunctionExpContext context) {
            String name = context.NAME().GetText();
            double result = 0;

            switch (name) {
                case "sqrt":
                    result = Math.Sqrt(Visit(context.expression()));
                    break;

                case "log":
                    result = Math.Log10(Visit(context.expression()));
                    break;
            }

            return result;
        }
    }
}