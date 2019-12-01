using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    internal partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitPostfixOperationExp(SkryptParser.PostfixOperationExpContext context) {
            var operationName = context.Operation.Text;

            var target = Visit(context.Target);
            var value = target;
            object result = value;

            switch (operationName) {
                case "++":
                    if (value is NumberInstance) {
                        var number = value as NumberInstance;
                        result = number.Value;
                        number.Value = (double)result + 1d;
                    }
                    break;
                case "--":
                    if (value is NumberInstance) {
                        var number = value as NumberInstance;
                        result = number.Value;
                        number.Value = (double)result - 1d;
                    }
                    break;
            }

            if (result is double) {
                result = _engine.CreateNumber((double)result);
            }

            if (result is InvalidOperation) {
                throw new InvalidOperationException($"No such operation: {value?.Name ?? "null"} {operationName}");
            }

            LastResult = (SkryptObject)result;

            return (SkryptObject)result;
        }
    }
}