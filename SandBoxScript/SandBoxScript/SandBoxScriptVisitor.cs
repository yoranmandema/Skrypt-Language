using System;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;
using SandBoxScript.ANTLR;
using System.Linq;
using SandBoxScript.Runtime;

namespace SandBoxScript {
    public partial class SandBoxScriptVisitor : SandBoxScriptBaseVisitor<BaseValue> {
        private readonly Engine _engine;

        public SandBoxScriptVisitor (Engine engine) {
            _engine = engine;
        }

        public override BaseValue VisitNumericAtomExp(SandBoxScriptParser.NumericAtomExpContext context) {
            var value = double.Parse(context.NUMBER().GetText(), System.Globalization.CultureInfo.InvariantCulture);
            var num = _engine.CreateNumber(value);
            return num;
        }

        public override BaseValue VisitStringExp(SandBoxScriptParser.StringExpContext context) {
            var str = (context.STRING().GetText());

            var instance = _engine.CreateString(str.Substring(1, str.Length - 2));
            return instance;
        }

        public override BaseValue VisitParenthesisExp(SandBoxScriptParser.ParenthesisExpContext context) {
            return Visit(context.expression());
        }

        public override BaseValue VisitNameExp(SandBoxScriptParser.NameExpContext context) {
            //throw new Exception("Name not found!");

            return _engine.Scope.Variables[context.NAME().GetText()];
        }

        public override BaseValue VisitBinaryOperationExp(SandBoxScriptParser.BinaryOperationExpContext context) {
            var operationName = context.Operation.Text;

            var left = Visit(context.Left);
            var right = Visit(context.Right);

            object result = new InvalidOperation();

            switch (operationName) {
                case "+":
                    result = _engine.expressionInterpreter.EvaluatePlusExpression(left, right);
                    break;
                case "*":
                    result = _engine.expressionInterpreter.EvaluateMultiplyExpression(left, right);
                    break;
                case "**":
                    result = _engine.expressionInterpreter.EvaluateExponentExpression(left, right);
                    break;
            }

            if (result.GetType() == typeof(InvalidOperation)) {
                throw new InvalidOperationException($"No such operation: {left.Name} {operationName} {right.Name}");
            }

            return (BaseValue)result;
        }

        public override BaseValue VisitMemberAccessExp(SandBoxScriptParser.MemberAccessExpContext context) {
            var obj = Visit(context.expression());
            var memberName = context.NAME().GetText();

            return obj.Members[memberName].Value;
        }

        public override BaseValue VisitFunctionCallExp(SandBoxScriptParser.FunctionCallExpContext context) {
            var function = Visit(context.expression(0));

            if (function.GetType() != typeof(FunctionObject)) {
                throw new Exception("Called object is not a function!");
            }

            var arguments = new BaseValue[context.expression().Length - 1];

            for (var i = 1; i < context.expression().Length; i++) {
                arguments[i - 1] = Visit(context.expression(i));
            }

            return ((FunctionObject)function).Function.Run(_engine, null, arguments);
        }
    }
}