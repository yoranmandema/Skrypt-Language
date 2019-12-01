using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    internal partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitPrefixOperationExp(SkryptParser.PrefixOperationExpContext context) {
            var operationName = context.Operation.Text;

            var target = Visit(context.Target);
            var value = target;
            object result = value;

            switch (operationName) {
                case "++":
                    if (value is NumberInstance) {
                        var number = value as NumberInstance;
                        result = number + 1d;
                        number.Value = (double)result;
                    }
                    break;
                case "--":
                    if (value is NumberInstance) {
                        var number = value as NumberInstance;
                        result = number - 1d;
                        number.Value = (double)result;
                    }
                    break;
                case "-":
                    result = _engine.ExpressionInterpreter.EvaluateMinusExpression(value);
                    break;
                case "~":
                    result = _engine.ExpressionInterpreter.EvaluateBitNotExpression(value);
                    break;
                case "!":
                    result = _engine.ExpressionInterpreter.EvaluateNotExpression(value);
                    break;
                case "typeof":
                    if (value is SkryptInstance instance) {
                        result = instance.TypeObject;
                    } else {
                        result = value;
                    }

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

            if (result is InvalidOperation) {
                var name = value == null ? "null" : typeof(SkryptType).IsAssignableFrom(value.GetType()) ? "type" : value.Name;

                _engine.ErrorHandler.FatalError(context.Target.Start, $"No such operation: {name} {operationName}");
            }

            LastResult = (SkryptObject)result;

            return (SkryptObject)result;
        }
    }
}