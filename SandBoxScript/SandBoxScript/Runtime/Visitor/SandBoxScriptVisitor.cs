using System;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;
using SandBoxScript.ANTLR;
using System.Linq;
using SandBoxScript.Runtime;

namespace SandBoxScript {
    public partial class SandBoxScriptVisitor : SandBoxScriptBaseVisitor<BaseValue> {
        private readonly Engine _engine;
        private BaseValue accessed;

        public SandBoxScriptVisitor (Engine engine) {
            _engine = engine;
        }

        public override BaseValue VisitImportStatement(SandBoxScriptParser.ImportStatementContext context) => DefaultResult;
        public override BaseValue VisitFunctionStatement(SandBoxScriptParser.FunctionStatementContext context) => DefaultResult;

        public override BaseValue VisitModuleStatement(SandBoxScriptParser.ModuleStatementContext context) {

            VisitChildren(context);

            foreach (var kv in context.Variables) {
                context.Module.Value.CreateProperty(kv.Key, kv.Value.Value);
            }

            return context.Module.Value;
        }

        public override BaseValue VisitWhileStatement(SandBoxScriptParser.WhileStatementContext context) {
            var block = context.stmntBlock().block();
            var expression = context.stmntBlock().expression();

            if (block != null) {
                while (Visit(context.Condition).IsTrue()) {
                    for (int i = 0; i < block.ChildCount; i++) {
                        var c = block.GetChild(i);

                        Visit(c);

                        if (context.JumpState == JumpState.Break || context.JumpState == JumpState.Continue || context.JumpState == JumpState.Return) {
                            break;
                        }
                    }

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
                while (Visit(context.Condition).IsTrue()) {
                    Visit(expression);

                    if (context.JumpState == JumpState.Break || context.JumpState == JumpState.Continue || context.JumpState == JumpState.Return) {
                        break;
                    }

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

            return DefaultResult;
        }

        public override BaseValue VisitContinueStatement([NotNull] SandBoxScriptParser.ContinueStatementContext context) {
            var loopCtx = context.Statement;

            if (loopCtx is SandBoxScriptParser.WhileStatementContext whileCtx)
                whileCtx.JumpState = JumpState.Continue;
            
            return DefaultResult;
        }

        public override BaseValue VisitBreakStatement([NotNull] SandBoxScriptParser.BreakStatementContext context) {
            var loopCtx = context.Statement;

            if (loopCtx is SandBoxScriptParser.WhileStatementContext whileCtx)
                whileCtx.JumpState = JumpState.Break;

            return DefaultResult;
        }

        public override BaseValue VisitIfStatement(SandBoxScriptParser.IfStatementContext context) {
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

        public override BaseValue VisitReturnStatement(SandBoxScriptParser.ReturnStatementContext context) {
            var fnCtx = context.Statement;
            var expression = context.expression();

            if (expression != null) {
                var returnValue = Visit(expression);

                fnCtx.ReturnValue = returnValue;
            }

            fnCtx.JumpState = JumpState.Return;

            return DefaultResult;
        }

        public override BaseValue VisitAssignNameStatement(SandBoxScriptParser.AssignNameStatementContext context) {
            context.name().variable.Value = Visit(context.expression());

            return DefaultResult;
        }

        public override BaseValue VisitAssignMemberStatement(SandBoxScriptParser.AssignMemberStatementContext context) {
            var target      = Visit(context.memberAccess().expression());
            var memberName  = context.memberAccess().NAME().GetText();

            target.SetProperty(memberName, Visit(context.expression()));

            return DefaultResult;
        }

        public override BaseValue VisitMemberAccessExp(SandBoxScriptParser.MemberAccessExpContext context) {
            var obj = Visit(context.expression());
            var memberName = context.NAME().GetText();

            var val = obj.Members[memberName].Value;
            

            if (val is GetPropertyInstance) {
                var newVal = (val as GetPropertyInstance).Property.Run(_engine, obj);

                accessed = newVal;
            } else {
                accessed = val;
            }

            return accessed;
        }

        public override BaseValue VisitNameExp(SandBoxScriptParser.NameExpContext context) {
            return context.name().variable.Value;
        }

        public override BaseValue VisitNumberLiteral(SandBoxScriptParser.NumberLiteralContext context) {
            var value = context.number().value;
            var num = _engine.CreateNumber(value);
            return num;
        }

        public override BaseValue VisitBooleanLiteral(SandBoxScriptParser.BooleanLiteralContext context) {
            var value = context.boolean().value;

            var boolean = _engine.CreateBoolean(value);
            return boolean;
        }

        public override BaseValue VisitStringLiteral(SandBoxScriptParser.StringLiteralContext context) {
            var value = context.@string().value;

            var str = _engine.CreateString(value);
            return str;
        }

        public override BaseValue VisitVector2Literal(SandBoxScriptParser.Vector2LiteralContext context) {
            var v = context.vector2();

            var x = (NumberInstance)Visit(v.X);
            var y = (NumberInstance)Visit(v.Y);

            var vec = _engine.CreateVector2(x, y);
            return vec;
        }

        public override BaseValue VisitVector3Literal(SandBoxScriptParser.Vector3LiteralContext context) {
            var v = context.vector3();

            var x = (NumberInstance)Visit(v.X);
            var y = (NumberInstance)Visit(v.Y);
            var z = (NumberInstance)Visit(v.Z);

            var vec = _engine.CreateVector3(x, y, z);
            return vec;
        }

        public override BaseValue VisitVector4Literal(SandBoxScriptParser.Vector4LiteralContext context) {
            var v = context.vector4();

            var x = (NumberInstance)Visit(v.X);
            var y = (NumberInstance)Visit(v.Y);
            var z = (NumberInstance)Visit(v.Z);
            var w = (NumberInstance)Visit(v.W);

            var vec = _engine.CreateVector4(x, y, z, w);
            return vec;
        }

        public override BaseValue VisitParenthesisExp(SandBoxScriptParser.ParenthesisExpContext context) {
            return Visit(context.expression());
        }

        public override BaseValue VisitBinaryOperationExp(SandBoxScriptParser.BinaryOperationExpContext context) {
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
                throw new InvalidOperationException($"No such operation: {left?.Name ?? "null"} {operationName} {right?.Name ?? "null"}");
            }

            return (BaseValue)result;
        }

        public override BaseValue VisitFunctionCallExp(SandBoxScriptParser.FunctionCallExpContext context) {
            var function = Visit(context.Function);

            if (function.GetType() != typeof(FunctionInstance)) {
                throw new Exception("Called object is not a function!");
            }

            var length = context.Arguments.expression().Length;

            var arguments = new BaseValue[length];

            for (var i = 0; i < length; i++) {
                arguments[i] = Visit(context.Arguments.expression(i));
            }

            var args = new Arguments(arguments);

            var returnValue = (function as FunctionInstance).Function.Run(_engine, accessed, args);

            return returnValue;
        }
    }
}