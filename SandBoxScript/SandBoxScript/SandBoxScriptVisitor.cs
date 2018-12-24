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

            return _engine.Create<NumberObject>(value);
        }

        public override BaseObject VisitParenthesisExp(SandBoxScriptParser.ParenthesisExpContext context) {
            return Visit(context.expression());
        }

        public override BaseObject VisitNameExp(SandBoxScriptParser.NameExpContext context) {
            //throw new Exception("Name not found!");

            return _engine.Create<NumberObject>(1);
        }

        public override BaseObject VisitOperationExp(SandBoxScriptParser.OperationExpContext context) {
            var operationName = context.Operation.Text;
            var result = default(BaseObject);
            var operation = default(IOperation);

            var left = Visit(context.expression(0));
            var leftType = left.Name;

            var isBinary = context.expression().Length == 2;

            if (isBinary) {
                var right = Visit(context.expression(1));
                var rightType = right.Name;

                var types = new[] { leftType, rightType };

                operation = Array.Find(left.StaticObject.Operations, x => {
                    if (x.GetType() != typeof(BinaryOperation)) return false;
                    if (x.Name != operationName) return false;

                    var op = (BinaryOperation)x;

                    if (op.LeftType != leftType) return false;
                    if (op.RightType != rightType) return false;

                    return true;
                });

                if (operation == null) {
                    operation = Array.Find(left.StaticObject.Operations, x => {
                        if (x.GetType() != typeof(BinaryOperation)) return false;
                        if (x.Name != operationName) return false;

                        var op = (BinaryOperation)x;

                        if (op.LeftType != leftType) return false;
                        if (op.RightType != rightType) return false;

                        return true;
                    });
                }

                result = operation.Function.Run(left, right);
            } else {
                operation = Array.Find(left.StaticObject.Operations, x => {
                    if (x.GetType() != typeof(UnaryOperation)) return false;
                    if (x.Name != operationName) return false;

                    var op = (UnaryOperation)x;

                    if (op.Type != leftType) return false;

                    return true;
                });

                result = operation.Function.Run(left);
            }

            return result;
        }

        public override BaseObject VisitFunctionCallExp(SandBoxScriptParser.FunctionCallExpContext context) {
            //var function = (Delegate)Visit(context.expression(0));

            var arguments = new object[context.expression().Length - 1];

            for (var i = 1; i < context.expression().Length; i++) {
                arguments[i - 1] = Visit(context.expression(i));
            }

            return null;
        }
    }
}