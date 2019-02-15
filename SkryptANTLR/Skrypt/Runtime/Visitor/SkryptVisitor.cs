using System;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;
using Skrypt.ANTLR;
using System.Linq;
using Skrypt.Runtime;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<BaseObject> {
        public BaseObject LastResult { get; private set; }

        private readonly Engine _engine;
        private BaseObject accessed;

        public SkryptVisitor (Engine engine) {
            _engine = engine;
        }

        public override BaseObject VisitImportStatement(SkryptParser.ImportStatementContext context) => DefaultResult;
        public override BaseObject VisitFunctionStatement(SkryptParser.FunctionStatementContext context) => DefaultResult;
        public override BaseObject VisitModuleStatement(SkryptParser.ModuleStatementContext context) => null;
        public override BaseObject VisitStructStatement(SkryptParser.StructStatementContext context) => null;
        public override BaseObject VisitTraitStatement(SkryptParser.TraitStatementContext context) => null;
        public override BaseObject VisitTraitImplStatement(SkryptParser.TraitImplStatementContext context) => null;

        void DoLoop(SkryptParser.BlockContext block, SkryptParser.ExpressionContext expression, ILoop context, Func<bool> cond, Action callback = null) {
            if (block != null) {
                while (cond()) {
                    for (int i = 0; i < block.ChildCount; i++) {
                        var c = block.GetChild(i);

                        Visit(c);

                        if (context.JumpState == JumpState.Break || context.JumpState == JumpState.Continue || context.JumpState == JumpState.Return) {
                            break;
                        }
                    }

                    callback?.Invoke();

                    if (context.JumpState == JumpState.Break || context.JumpState == JumpState.Return) {
                        context.JumpState = JumpState.None;
                        break;
                    }
                    else if (context.JumpState == JumpState.Continue) {
                        context.JumpState = JumpState.None;
                        continue;
                    }
                }
            }
            else if (expression != null) {
                while (cond()) {
                    Visit(expression);

                    callback?.Invoke();

                    if (context.JumpState == JumpState.Break || context.JumpState == JumpState.Return) {
                        context.JumpState = JumpState.None;
                        break;
                    }
                    else if (context.JumpState == JumpState.Continue) {
                        context.JumpState = JumpState.None;
                        continue;
                    }
                }
            }
        }

        public override BaseObject VisitWhileStatement(SkryptParser.WhileStatementContext context) {
            var block = context.stmntBlock().block();
            var expression = context.stmntBlock().expression();

            DoLoop(block, expression, context, () => {
                return Visit(context.Condition).IsTrue();
            });

            return DefaultResult;
        }

        public override BaseObject VisitForStatement(SkryptParser.ForStatementContext context) {
            var block = context.stmntBlock().block();
            var expression = context.stmntBlock().expression();

            Visit(context.Instantiator);

            DoLoop(block, expression, context, 
            () => {
                var result = Visit(context.Condition).IsTrue();

                return result;
            }, 
            () => {
                Visit(context.Modifier);
            });

            return DefaultResult;
        }

        public override BaseObject VisitContinueStatement([NotNull] SkryptParser.ContinueStatementContext context) {
            var loopCtx = context.Statement;

            if (loopCtx is SkryptParser.WhileStatementContext whileCtx)
                whileCtx.JumpState = JumpState.Continue;
            
            return DefaultResult;
        }

        public override BaseObject VisitBreakStatement([NotNull] SkryptParser.BreakStatementContext context) {
            var loopCtx = context.Statement;

            if (loopCtx is SkryptParser.WhileStatementContext whileCtx)
                whileCtx.JumpState = JumpState.Break;

            return DefaultResult;
        }

        public override BaseObject VisitIfStatement(SkryptParser.IfStatementContext context) {
            var isTrue = false;

            isTrue = Visit(context.@if().Condition).IsTrue();

            if (isTrue) {
                Visit(context.@if().stmntBlock());

                return null;
            } else if (context.elseif().Length > 0) {
                foreach (var stmnt in context.elseif()) {
                    isTrue = Visit(stmnt.Condition).IsTrue();

                    if (isTrue) {
                        Visit(stmnt.stmntBlock());

                        return null;
                    }
                }
            }

            if (context.@else() != null) {
                Visit(context.@else().stmntBlock());
            }

            return DefaultResult;
        }

        public override BaseObject VisitReturnStatement(SkryptParser.ReturnStatementContext context) {
            var fnCtx = context.Statement;
            var expression = context.expression();

            if (expression != null) {
                var returnValue = Visit(expression);

                fnCtx.ReturnValue = returnValue;
            }

            fnCtx.JumpState = JumpState.Return;

            return DefaultResult;
        }

        public override BaseObject VisitAssignNameStatement(SkryptParser.AssignNameStatementContext context) {
            if (context.name().variable.IsConstant) {
                _engine.ErrorHandler.FatalError(context.Start, "Constant cannot be redefined.");
            }

            var value = Visit(context.expression());

            if (value is IValue noref) value = noref.Copy();

            context.name().variable.Value = value;
            
            return DefaultResult;
        }

        public override BaseObject VisitAssignMemberStatement(SkryptParser.AssignMemberStatementContext context) {
            var target      = Visit(context.memberAccess().expression());
            var memberName  = context.memberAccess().NAME().GetText();

            var property = target.GetProperty(memberName);

            if (property.IsPrivate && property.DefinitionBlock != null) {
                var parent = context.Parent;
                var canAccess = false;

                while (parent != null) {
                    if (parent == property.DefinitionBlock) {
                        canAccess = true;
                    }

                    parent = parent.Parent;
                }

                if (!canAccess) {
                    _engine.ErrorHandler.FatalError(context.memberAccess().Start, $"Private property {memberName} is not accessible in the current context.");
                }
            }

            var value = Visit(context.expression());

            if (value is IValue noref) value = noref.Copy();

            target.SetProperty(memberName, value);

            return DefaultResult;
        }

        public override BaseObject VisitAssignComputedMemberStatement([NotNull] SkryptParser.AssignComputedMemberStatementContext context) {
            var lhs = context.memberAccessComp();

            var obj = Visit(lhs.expression(0));
            var index = Visit(lhs.expression(1));
            var value = Visit(context.expression());

            if (value is IValue noref) value = noref.Copy();

            if (obj is ArrayInstance arrayInstance) {
                return arrayInstance.Set(index, value);
            }

            _engine.ErrorHandler.FatalError(lhs.expression(0).Start, "Expected array instance.");

            return null;
        }

        public override BaseObject VisitMemberAccessExp(SkryptParser.MemberAccessExpContext context) {
            var obj = Visit(context.expression());
            var memberName = context.NAME().GetText();

            var property = obj.GetProperty(memberName);

            if (property.IsPrivate && property.DefinitionBlock != null) {
                var parent = context.Parent;
                var canAccess = false;

                while (parent != null) {
                    if (parent == property.DefinitionBlock) {
                        canAccess = true;
                    }

                    parent = parent.Parent;
                }

                if (!canAccess) {
                    _engine.ErrorHandler.FatalError(context.NAME().Symbol, $"Private property {memberName} is not accessible in the current context.");
                }
            }

            var value = property.Value;

            if (value is GetPropertyInstance) {
                var newVal = (value as GetPropertyInstance).Property.Run(_engine, obj);

                value = newVal;
            }

            if (value is IValue noref) value = noref.Copy();

            accessed = obj;

            LastResult = accessed;

            return value;
        }

        public override BaseObject VisitComputedMemberAccessExp([NotNull] SkryptParser.ComputedMemberAccessExpContext context) {
            var obj = Visit(context.expression(0));
            var index = Visit(context.expression(1));

            if (obj is StringInstance stringInstance) {
                var value = stringInstance.Get(index);

                if (value is IValue noref) value = noref.Copy();

                return value;
            } else if (obj is ArrayInstance arrayInstance) {
                var value = arrayInstance.Get(index);

                if (value is IValue noref) value = noref.Copy();

                return value;
            }

            _engine.ErrorHandler.FatalError(context.expression(0).Start, "Expected string or array instance.");

            return null;
        }

        public override BaseObject VisitNameExp(SkryptParser.NameExpContext context) {
            var value = context.name().variable.Value;

            LastResult = value;

            return context.name().variable.Value;
        }

        public override BaseObject VisitNumberLiteral(SkryptParser.NumberLiteralContext context) {
            var value = context.number().value;
            var num = _engine.CreateNumber(value);

            LastResult = num;

            return num;
        }

        public override BaseObject VisitBooleanLiteral(SkryptParser.BooleanLiteralContext context) {
            var value = context.boolean().value;
            var boolean = _engine.CreateBoolean(value);

            LastResult = boolean;

            return boolean;
        }

        public override BaseObject VisitNullLiteral([NotNull] SkryptParser.NullLiteralContext context) {
            LastResult = null;

            return null;
        }

        public override BaseObject VisitStringLiteral(SkryptParser.StringLiteralContext context) {
            var value = context.@string().value;
            var str = _engine.CreateString(value);

            LastResult = str;

            return str;
        }

        public override BaseObject VisitVector2Literal(SkryptParser.Vector2LiteralContext context) {
            var v = context.vector2();

            var x = (NumberInstance)Visit(v.X);
            var y = (NumberInstance)Visit(v.Y);

            var vec = _engine.CreateVector2(x, y);

            LastResult = vec;

            return vec;
        }

        public override BaseObject VisitVector3Literal(SkryptParser.Vector3LiteralContext context) {
            var v = context.vector3();

            var x = (NumberInstance)Visit(v.X);
            var y = (NumberInstance)Visit(v.Y);
            var z = (NumberInstance)Visit(v.Z);

            var vec = _engine.CreateVector3(x, y, z);

            LastResult = vec;

            return vec;
        }

        public override BaseObject VisitVector4Literal(SkryptParser.Vector4LiteralContext context) {
            var v = context.vector4();

            var x = (NumberInstance)Visit(v.X);
            var y = (NumberInstance)Visit(v.Y);
            var z = (NumberInstance)Visit(v.Z);
            var w = (NumberInstance)Visit(v.W);

            var vec = _engine.CreateVector4(x, y, z, w);

            LastResult = vec;

            return vec;
        }

        public override BaseObject VisitArrayLiteral([NotNull] SkryptParser.ArrayLiteralContext context) {
            var a = context.array();

            var expressions = a.expressionGroup().expression();
            var values = new BaseObject[expressions.Length];

            for (int i = 0; i < values.Length; i++) {
                values[i] = Visit(expressions[i]);
            }

            var array = _engine.CreateArray(values);

            LastResult = array;
             
            return array;
        }

        public override BaseObject VisitFunctionLiteral([NotNull] SkryptParser.FunctionLiteralContext context) {
            var fn = context.fnLiteral().value;

            LastResult = fn;

            return fn;
        }

        public override BaseObject VisitParenthesisExp(SkryptParser.ParenthesisExpContext context) {
            return Visit(context.expression());
        }

        public override BaseObject VisitPostfixOperationExp(SkryptParser.PostfixOperationExpContext context) {
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

            LastResult = (BaseObject)result;

            return (BaseObject)result;
        }

        public override BaseObject VisitPrefixOperationExp(SkryptParser.PrefixOperationExpContext context) {
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
                    result = _engine.expressionInterpreter.EvaluateMinusExpression(value);
                    break;
                case "~":
                    result = _engine.expressionInterpreter.EvaluateBitNotExpression(value);
                    break;
                case "!":
                    result = _engine.expressionInterpreter.EvaluateNotExpression(value);
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
                var name = typeof(BaseType).IsAssignableFrom(value.GetType()) ? "type" : value?.Name;

                _engine.ErrorHandler.FatalError(context.Target.Start, $"No such operation: {name ?? "null"} {operationName}");
            }

            LastResult = (BaseObject)result;

            return (BaseObject)result;
        }

        public override BaseObject VisitBinaryOperationExp(SkryptParser.BinaryOperationExpContext context) {
            var operationName = context.Operation.Text;

            var left = Visit(context.Left);
            var right = Visit(context.Right);

            object result = new InvalidOperation();

            switch (operationName) {
                case "+":
                    result = _engine.expressionInterpreter.EvaluatePlusExpression(left, right);
                    break;
                case "-":
                    result = _engine.expressionInterpreter.EvaluateSubtractExpression(left, right);
                    break;
                case "*":
                    result = _engine.expressionInterpreter.EvaluateMultiplyExpression(left, right);
                    break;
                case "/":
                    result = _engine.expressionInterpreter.EvaluateDivideExpression(left, right);
                    break;
                case "%":
                    result = _engine.expressionInterpreter.EvaluateRemainderExpression(left, right);
                    break;
                case "**":
                    result = _engine.expressionInterpreter.EvaluateExponentExpression(left, right);
                    break;
                case "<":
                    result = _engine.expressionInterpreter.EvaluateLessExpression(left, right);
                    break;
                case "<=":
                    result = _engine.expressionInterpreter.EvaluateLessEqualExpression(left, right);
                    break;
                case ">":
                    result = _engine.expressionInterpreter.EvaluateGreaterExpression(left, right);
                    break;
                case ">=":
                    result = _engine.expressionInterpreter.EvaluateGreaterEqualExpression(left, right);
                    break;
                case "==":
                    result = _engine.expressionInterpreter.EvaluateEqualExpression(left, right);
                    break;
                case "!=":
                    result = _engine.expressionInterpreter.EvaluateNotEqualExpression(left, right);
                    break;
                case "is":
                    result = _engine.expressionInterpreter.EvaluateIsExpression(left, right);
                    break;
                case "and":
                    result = _engine.expressionInterpreter.EvaluateAndExpression(left, right);
                    break;
                case "or":
                    result = _engine.expressionInterpreter.EvaluateOrExpression(left, right);
                    break;
                case "&":
                    result = _engine.expressionInterpreter.EvaluateBitAndExpression(left, right);
                    break;
                case "^":
                    result = _engine.expressionInterpreter.EvaluateBitXOrExpression(left, right);
                    break;
                case "|":
                    result = _engine.expressionInterpreter.EvaluateBitOrExpression(left, right);
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
                var lname = typeof(BaseType).IsAssignableFrom(left.GetType()) ? "type" : left?.Name;
                var rname = typeof(BaseType).IsAssignableFrom(right.GetType()) ? "type" : right?.Name;

                _engine.ErrorHandler.FatalError(context.Left.Start, $"No such operation: {lname ?? "null"} {operationName} {rname ?? "null"}.");
            }


            LastResult = (BaseObject)result;

            return (BaseObject)result;
        }

        public override BaseObject VisitFunctionCallExp(SkryptParser.FunctionCallExpContext context) {
            var function = Visit(context.Function);
            var isConstructor = false;
            var returnValue = DefaultResult;

            if (typeof(BaseType).IsAssignableFrom(function.GetType())) {
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
            } else {
                returnValue = (function as FunctionInstance).Function.Run(_engine, accessed, args);
            }

            LastResult = returnValue;

            return returnValue;
        }
    }
}