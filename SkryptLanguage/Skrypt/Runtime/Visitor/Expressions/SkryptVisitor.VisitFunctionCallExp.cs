using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    internal partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitFunctionCallExp(SkryptParser.FunctionCallExpContext context) {
            var arguments = new SkryptObject[context.Arguments.expression().Length];

            for (var i = 0; i < arguments.Length; i++) {
                arguments[i] = Visit(context.Arguments.expression(i));
            }

            var args = new Arguments(arguments);

            var function = Visit(context.Function);
            var isConstructor = false;
            SkryptObject returnValue;

            if (function is SkryptType) {
                isConstructor = true;
            }
            else if (!(function is FunctionInstance)) {
                _engine.ErrorHandler.FatalError(context.Function.Start, "Called object is not a function.");
            }

            _engine.CallStack.Push(new Call {
                name = context.Function.GetText(),
                column = context.Function.Start.Column,
                line = context.Function.Start.Line,
                file = (function as IDefined)?.File,
                callFile = context.CallFile,
                token = context.Start
            });

            if (isConstructor) {
                returnValue = ((SkryptType)function).Construct(args);
            }
            else {
                var functionInstance = function as FunctionInstance;

                returnValue = functionInstance.Function.Run(_engine, accessed ?? functionInstance.Self, args);
            }

            accessed = null;

            _engine.CallStack.Pop();

            LastResult = returnValue;

            return returnValue;
        }
    }
}
