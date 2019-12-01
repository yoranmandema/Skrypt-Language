using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;
using System.Linq;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {

        public SkryptObject EvaluateExpression (string operationName, SkryptObject left, SkryptObject right, IToken token) {
            object result = new InvalidOperation();

            switch (operationName) {
                case "+":
                    result = _engine.ExpressionInterpreter.EvaluatePlusExpression(left, right);

                    break;
                case "-":
                    result = _engine.ExpressionInterpreter.EvaluateSubtractExpression(left, right);

                    break;
                case "*":
                    result = _engine.ExpressionInterpreter.EvaluateMultiplyExpression(left, right);
                    break;
                case "/":
                    result = _engine.ExpressionInterpreter.EvaluateDivideExpression(left, right);
                    break;
                case "%":
                    result = _engine.ExpressionInterpreter.EvaluateRemainderExpression(left, right);
                    break;
                case "**":
                    result = _engine.ExpressionInterpreter.EvaluateExponentExpression(left, right);
                    break;
                case "<":
                    result = _engine.ExpressionInterpreter.EvaluateLessExpression(left, right);
                    break;
                case "<=":
                    result = _engine.ExpressionInterpreter.EvaluateLessEqualExpression(left, right);
                    break;
                case ">":
                    result = _engine.ExpressionInterpreter.EvaluateGreaterExpression(left, right);
                    break;
                case ">=":
                    result = _engine.ExpressionInterpreter.EvaluateGreaterEqualExpression(left, right);
                    break;
                case "==":
                    result = _engine.ExpressionInterpreter.EvaluateEqualExpression(left, right);
                    break;
                case "!=":
                    result = _engine.ExpressionInterpreter.EvaluateNotEqualExpression(left, right);
                    break;
                case "is":
                    result = _engine.ExpressionInterpreter.EvaluateIsExpression(left, right);
                    break;
                case "&&":
                    result = _engine.ExpressionInterpreter.EvaluateAndExpression(left, right);
                    break;
                case "||":
                    result = _engine.ExpressionInterpreter.EvaluateOrExpression(left, right);
                    break;
                case "&":
                    result = _engine.ExpressionInterpreter.EvaluateBitAndExpression(left, right);
                    break;
                case "^":
                    result = _engine.ExpressionInterpreter.EvaluateBitXOrExpression(left, right);
                    break;
                case "|":
                    result = _engine.ExpressionInterpreter.EvaluateBitOrExpression(left, right);
                    break;
                case "<<":
                    result = _engine.ExpressionInterpreter.EvaluateBitShiftLExpression(left, right);
                    break;
                case ">>":
                    result = _engine.ExpressionInterpreter.EvaluateBitShiftRExpression(left, right);
                    break;
                case ">>>":
                    result = _engine.ExpressionInterpreter.EvaluateBitShiftURExpression(left, right);
                    break;
            }

            if (result is bool) {
                result = _engine.CreateBoolean((bool)result);
            }

            if (result is double) {
                result = _engine.CreateNumber((double)result);
            }

            if (result is int) {
                result = _engine.CreateNumber((int)result);
            }

            if (result is string) {
                result = _engine.CreateString((string)result);
            }

            if (left != null) {
                if (left.AsType<SkryptInstance>().HasTrait<SubtractableTrait>()) {
                    result = EvaluateTraitOperator("Sub", left, right);
                }
                else if (left.AsType<SkryptInstance>().HasTrait<AddableTrait>()) {
                    result = EvaluateTraitOperator("Add", left, right);
                }
                else if (left.AsType<SkryptInstance>().HasTrait<MultiplicableTrait>()) {
                    result = EvaluateTraitOperator("Mul", left, right);
                }
                else if (left.AsType<SkryptInstance>().HasTrait<DividableTrait>()) {
                    result = EvaluateTraitOperator("Div", left, right);
                }
            }

            if (result is InvalidOperation) {
                if (result is InvalidOperation) {
                    var lname = left == null ? "null" : typeof(SkryptType).IsAssignableFrom(left.GetType()) ? "type" : left.Name;
                    var rname = right == null ? "null" : typeof(SkryptType).IsAssignableFrom(right.GetType()) ? "type" : right.Name;

                    _engine.ErrorHandler.FatalError(token, $"No such operation: {lname} {operationName} {rname}.");
                }
            }

            return (SkryptObject)result;
        }

        public override SkryptObject VisitBinaryOperationExp(SkryptParser.BinaryOperationExpContext context) {
            var operationName = context.Operation.Text;

            var left = Visit(context.Left);
            var right = Visit(context.Right);

            var result = EvaluateExpression(operationName, left, right, context.Left.start);

            LastResult = result;

            return result;
        }

        private object EvaluateTraitOperator (string name, SkryptObject left, SkryptObject right) {
            return left.AsType<SkryptInstance>().GetProperty(name).value.AsType<FunctionInstance>().RunOnSelf(left, left, right);
        }
    }
}