using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<BaseObject> {
        public override BaseObject VisitFunctionCallExp(SkryptParser.FunctionCallExpContext context) {
            var function = Visit(context.Function);
            var isConstructor = false;
            var returnValue = DefaultResult;

            if (function is BaseType) {
                isConstructor = true;
            }
            else if (!(function is FunctionInstance)) {
                _engine.ErrorHandler.FatalError(context.Function.Start, "Called object is not a function.");
            }

            var length = context.Arguments.expression().Length;

            var arguments = new BaseObject[length];

            for (var i = 0; i < length; i++) {
                arguments[i] = Visit(context.Arguments.expression(i));
            }

            var args = new Arguments(arguments);

            if (isConstructor) {
                returnValue = (function as BaseType).Construct(args);
            }
            else {
                returnValue = (function as FunctionInstance).Function.Run(_engine, accessed, args);
            }

            LastResult = returnValue;

            return returnValue;
        }
    }
}
