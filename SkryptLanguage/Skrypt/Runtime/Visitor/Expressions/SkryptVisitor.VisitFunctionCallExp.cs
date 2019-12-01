using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    internal partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public override SkryptObject VisitFunctionCallExp(SkryptParser.FunctionCallExpContext context) {
            var length = context.Arguments.expression().Length;

            var arguments = new SkryptObject[length];

            for (var i = 0; i < length; i++) {
                arguments[i] = Visit(context.Arguments.expression(i));
            }

            var args = new Arguments(arguments);

            var function = Visit(context.Function);
            var isConstructor = false;
            var returnValue = DefaultResult;

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
                returnValue = (function as SkryptType).Construct(args);
            }
            else {
                var functionInstance = function as FunctionInstance;
                var self = accessed ?? functionInstance.Self;

                returnValue = functionInstance.Function.Run(_engine, self, args);
            }

            accessed = null;

            _engine.CallStack.Pop();

            LastResult = returnValue;

            return returnValue;
        }
    }
}
