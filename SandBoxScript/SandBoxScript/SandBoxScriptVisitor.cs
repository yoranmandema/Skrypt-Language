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

        public override BaseValue VisitImportStatement(SandBoxScriptParser.ImportStatementContext context) {
            var obj = Visit(context.Target);

            foreach (var m in obj.Members) {
                var v = m.Value;

                _engine.Scope.SetVariable(m.Key, v.Value);
            }

            return null;
        }

        public override BaseValue VisitAssignNameStatement(SandBoxScriptParser.AssignNameStatementContext context) {
            _engine.Scope.SetVariable(context.NAME().GetText(), Visit(context.expression()));

            return null;
        }

        public override BaseValue VisitAssignMemberStatement(SandBoxScriptParser.AssignMemberStatementContext context) {
            var target      = Visit(context.memberAccess().expression());
            var memberName  = context.memberAccess().NAME().GetText();

            target.SetProperty(memberName, Visit(context.expression()));

            return null;
        }

        public override BaseValue VisitNumberLiteral(SandBoxScriptParser.NumberLiteralContext context) {
            var value = context.number().value;
            var num = _engine.CreateNumber(value);
            return num;
        }

        public override BaseValue VisitStringLiteral(SandBoxScriptParser.StringLiteralContext context) {
            var value = context.@string().value;

            var str = _engine.CreateString(value);
            return str;
        }

        public override BaseValue VisitParenthesisExp(SandBoxScriptParser.ParenthesisExpContext context) {
            return Visit(context.expression());
        }

        public override BaseValue VisitNameExp(SandBoxScriptParser.NameExpContext context) {
            return _engine.Scope.GetVariable(context.NAME().GetText());
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

            if (result is InvalidOperation) {
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
            var function = Visit(context.Function);

            if (function.GetType() != typeof(FunctionInstance)) {
                throw new Exception("Called object is not a function!");
            }

            var arguments = new BaseValue[context.Arguments.expression().Length];

            for (var i = 0; i < context.Arguments.expression().Length; i++) {
                arguments[i] = Visit(context.Arguments.expression(i));
            }

            return ((FunctionInstance)function).Function.Run(_engine, null, new Arguments(arguments));
        }
    }
}