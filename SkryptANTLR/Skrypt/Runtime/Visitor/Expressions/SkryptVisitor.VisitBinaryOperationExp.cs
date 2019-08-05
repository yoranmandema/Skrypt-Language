using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;
using System.Linq;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<BaseObject> {
        public override BaseObject VisitBinaryOperationExp(SkryptParser.BinaryOperationExpContext context) {
            var operationName = context.Operation.Text;

            var left = Visit(context.Left);
            var right = Visit(context.Right);

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
                case "and":
                    result = _engine.ExpressionInterpreter.EvaluateAndExpression(left, right);
                    break;
                case "or":
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

            if (result is InvalidOperation) {
                if (left.AsType<BaseInstance>().TypeObject.Traits.OfType<SubtractableTrait>().Any()) {
                    result = EvaluateTraitOperator("Sub", left, right);
                }
                else if (left.AsType<BaseInstance>().TypeObject.Traits.OfType<AddableTrait>().Any()) {
                    result = EvaluateTraitOperator("Add", left, right);
                }
                else if (left.AsType<BaseInstance>().TypeObject.Traits.OfType<MultiplicableTrait>().Any()) {
                    result = EvaluateTraitOperator("Mul", left, right);
                }
                else if (left.AsType<BaseInstance>().TypeObject.Traits.OfType<DividableTrait>().Any()) {
                    result = EvaluateTraitOperator("Div", left, right);
                }

                if (result is InvalidOperation) {
                    var lname = left == null ? "null" : typeof(BaseType).IsAssignableFrom(left.GetType()) ? "type" : left.Name;
                    var rname = right == null ? "null" : typeof(BaseType).IsAssignableFrom(right.GetType()) ? "type" : right.Name;

                    _engine.ErrorHandler.FatalError(context.Left.Start, $"No such operation: {lname} {operationName} {rname}.");
                }
            }


            LastResult = (BaseObject)result;

            return (BaseObject)result;
        }

        private object EvaluateTraitOperator (string name, BaseObject left, BaseObject right) {
            return left.AsType<BaseInstance>().GetProperty(name).Value.AsType<FunctionInstance>().RunOnSelf(left, right);
        }
    }
}