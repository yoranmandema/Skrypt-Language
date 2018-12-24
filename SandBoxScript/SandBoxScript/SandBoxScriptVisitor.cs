using System;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;
using SandBoxScript.ANTLR;
using System.Linq;

namespace SandBoxScript {
    public class SandBoxScriptVisitor : SandBoxScriptBaseVisitor<BaseObject> {
        private readonly Engine _engine;

        public SandBoxScriptVisitor (Engine engine) {
            _engine = engine;
        }

        public override BaseObject VisitNumericAtomExp(SandBoxScriptParser.NumericAtomExpContext context) {
            var value = double.Parse(context.NUMBER().GetText(), System.Globalization.CultureInfo.InvariantCulture);
            var num = _engine.Create<NumberObject>(value);
            return num;
        }

        public override BaseObject VisitParenthesisExp(SandBoxScriptParser.ParenthesisExpContext context) {
            return Visit(context.expression());
        }

        public override BaseObject VisitNameExp(SandBoxScriptParser.NameExpContext context) {
            //throw new Exception("Name not found!");

            return _engine.Scope.Variables[context.NAME().GetText()];
        }

        public override BaseObject VisitBinaryOperationExp(SandBoxScriptParser.BinaryOperationExpContext context) {
            var operationName = context.Operation.Text;
            var result = default(BaseObject);
            var operation = default(IOperation);

            var left = Visit(context.expression(0));
            var leftType = left.Name;

            var right = Visit(context.expression(1));
            var rightType = right.Name;

            operation = left.Operations.Find(x => {
                if (x.GetType() != typeof(BinaryOperation)) return false;
                if (x.Name != operationName) return false;

                var op = (BinaryOperation)x;

                if (op.LeftType != leftType) return false;
                if (op.RightType != rightType) return false;

                return true;
            });

            if (operation == null) {
                operation = left.Operations.Find(x => {
                    if (x.GetType() != typeof(BinaryOperation)) return false;
                    if (x.Name != operationName) return false;

                    var op = (BinaryOperation)x;

                    if (op.LeftType != leftType) return false;
                    if (op.RightType != rightType) return false;

                    return true;
                });
            }

            if (operation == null) {
                throw new InvalidOperationException($"No such operation: {leftType} {operationName} {rightType}");
            }

            result = operation.Function.Run(_engine, null, new[] { left, right });

            return result;
        }

        public override BaseObject VisitMemberAccessExp(SandBoxScriptParser.MemberAccessExpContext context) {
            var obj = Visit(context.expression());
            var memberName = context.NAME().GetText();

            return obj.Members[memberName].Value;
        }

        public override BaseObject VisitFunctionCallExp(SandBoxScriptParser.FunctionCallExpContext context) {
            var function = Visit(context.expression(0));

            if (function.GetType() != typeof(FunctionObject)) {
                throw new Exception("Called object is not a function!");
            }

            var arguments = new BaseObject[context.expression().Length - 1];

            for (var i = 1; i < context.expression().Length; i++) {
                arguments[i - 1] = Visit(context.expression(i));
            }

            return ((FunctionObject)function).Function.Run(_engine, null, arguments);
        }
    }
}