using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript.Runtime {
    sealed class ExpressionInterpreter {
        private readonly Engine _engine;

        public ExpressionInterpreter (Engine engine) {
            _engine = engine;
        }

        public object EvaluatePlusExpression (BaseValue left, BaseValue right) {
            if (left is NumberInstance && right is NumberInstance) {
                return _engine.CreateNumber((NumberInstance)left + (NumberInstance)right);
            }

            if (left is StringInstance) {
                return _engine.CreateString(((StringInstance)left).Value + right);
            }

            if (right is StringInstance) {
                return _engine.CreateString(left + ((StringInstance)right).Value);
            }

            if (left is VectorInstance && right is VectorInstance) {
                return VectorInstance.ComponentMath(_engine, left as VectorInstance, right as VectorInstance, (x, y) => x + y);
            }

            if (left is NumberInstance && right is VectorInstance) {
                return VectorInstance.ComponentMathNumeric(_engine, right as VectorInstance, (x) => (left as NumberInstance) + x);
            }

            if (left is VectorInstance && right is NumberInstance) {
                return VectorInstance.ComponentMathNumeric(_engine, left as VectorInstance, (x) => x + (right as NumberInstance));
            }

            return new InvalidOperation();
        }

        public object EvaluateSubtractExpression(BaseValue left, BaseValue right) {
            if (left is NumberInstance && right is NumberInstance) {
                return _engine.CreateNumber((NumberInstance)left - (NumberInstance)right);
            }

            if (left is VectorInstance && right is VectorInstance) {
                return VectorInstance.ComponentMath(_engine, left as VectorInstance, right as VectorInstance, (x,y) => x - y);
            }

            if (left is VectorInstance && right is NumberInstance) {
                return VectorInstance.ComponentMathNumeric(_engine, left as VectorInstance, (x) => x - (right as NumberInstance));
            }

            return new InvalidOperation();
        }

        public object EvaluateMultiplyExpression(BaseValue left, BaseValue right) {
            if (left is NumberInstance && right is NumberInstance) {
                return _engine.CreateNumber((NumberInstance)left * (NumberInstance)right);
            }

            if (left is VectorInstance && right is VectorInstance) {
                return VectorInstance.ComponentMath(_engine, left as VectorInstance, right as VectorInstance, (x, y) => x * y);
            }

            if (left is NumberInstance && right is VectorInstance) {
                return VectorInstance.ComponentMathNumeric(_engine, right as VectorInstance, (x) => (left as NumberInstance) * x);
            }

            if (left is VectorInstance && right is NumberInstance) {
                return VectorInstance.ComponentMathNumeric(_engine, left as VectorInstance, (x) => x * (right as NumberInstance));
            }

            return new InvalidOperation();
        }

        public object EvaluateDivideExpression(BaseValue left, BaseValue right) {
            if (left is NumberInstance && right is NumberInstance) {
                return _engine.CreateNumber((NumberInstance)left / (NumberInstance)right);
            }

            if (left is VectorInstance && right is VectorInstance) {
                return VectorInstance.ComponentMath(_engine, left as VectorInstance, right as VectorInstance, (x, y) => x / y);
            }

            if (left is VectorInstance && right is NumberInstance) {
                return VectorInstance.ComponentMathNumeric(_engine, left as VectorInstance, (x) => x / (right as NumberInstance));
            }
    
            return new InvalidOperation();
        }

        public object EvaluateRemainderExpression(BaseValue left, BaseValue right) {
            if (left is NumberInstance && right is NumberInstance) {
                return _engine.CreateNumber((NumberInstance)left % (NumberInstance)right);
            }

            if (left is VectorInstance && right is VectorInstance) {
                return VectorInstance.ComponentMath(_engine, left as VectorInstance, right as VectorInstance, (x, y) => x % y);
            }

            if (left is VectorInstance && right is NumberInstance) {
                return VectorInstance.ComponentMathNumeric(_engine, left as VectorInstance, (x) => x % (right as NumberInstance));
            }

            return new InvalidOperation();
        }

        public object EvaluateExponentExpression(BaseValue left, BaseValue right) {
            if (left is NumberInstance && right is NumberInstance) {
                return _engine.CreateNumber(Math.Pow((NumberInstance)left, (NumberInstance)right));
            }

            return new InvalidOperation();
        }

        public object EvaluateLessExpression(BaseValue left, BaseValue right) {
            if (left is NumberInstance && right is NumberInstance) {
                return _engine.CreateBoolean(((NumberInstance)left).Value < ((NumberInstance)right).Value);
            }

            return new InvalidOperation();
        }

        public object EvaluateLessEqualExpression(BaseValue left, BaseValue right) {
            if (left is NumberInstance && right is NumberInstance) {
                return _engine.CreateBoolean(((NumberInstance)left).Value <= ((NumberInstance)right).Value);
            }

            return new InvalidOperation();
        }

        public object EvaluateGreaterExpression(BaseValue left, BaseValue right) {
            if (left is NumberInstance && right is NumberInstance) {
                return _engine.CreateBoolean(((NumberInstance)left).Value > ((NumberInstance)right).Value);
            }

            return new InvalidOperation();
        }

        public object EvaluateGreaterEqualExpression(BaseValue left, BaseValue right) {
            if (left is NumberInstance && right is NumberInstance) {
                return _engine.CreateBoolean(((NumberInstance)left).Value >= ((NumberInstance)right).Value);
            }

            return new InvalidOperation();
        }

        public object EvaluateEqualExpression(BaseValue left, BaseValue right) {
            if (left is NumberInstance && right is NumberInstance) {
                return _engine.CreateBoolean(((NumberInstance)left).Value == ((NumberInstance)right).Value);
            }
            else if (left is StringInstance && right is StringInstance) {
                return _engine.CreateBoolean(((StringInstance)left).Value == ((StringInstance)right).Value);
            }
            else if (left is BooleanInstance && right is BooleanInstance) {
                return _engine.CreateBoolean(((BooleanInstance)left).Value == ((BooleanInstance)right).Value);
            }
            else {
                return left == right;
            }
        }

        public object EvaluateNotEqualExpression(BaseValue left, BaseValue right) {
            if (left is NumberInstance && right is NumberInstance) {
                return _engine.CreateBoolean(((NumberInstance)left).Value != ((NumberInstance)right).Value);
            }
            else if (left is StringInstance && right is StringInstance) {
                return _engine.CreateBoolean(((StringInstance)left).Value != ((StringInstance)right).Value);
            }
            else if (left is BooleanInstance && right is BooleanInstance) {
                return _engine.CreateBoolean(((BooleanInstance)left).Value != ((BooleanInstance)right).Value);
            }
            else {
                return left != right;
            }
        }

        public object EvaluateAndExpression(BaseValue left, BaseValue right) {
            if (left is BooleanInstance && right is BooleanInstance) {
                return _engine.CreateBoolean(((BooleanInstance)left).Value && ((BooleanInstance)right).Value);
            }

            return new InvalidOperation();
        }

        public object EvaluateOrExpression(BaseValue left, BaseValue right) {
            if (left is BooleanInstance && right is BooleanInstance) {
                return _engine.CreateBoolean(((BooleanInstance)left).Value && ((BooleanInstance)right).Value);
            }

            return new InvalidOperation();
        }

        public object EvaluateBitAndExpression(BaseValue left, BaseValue right) {
            if (left is NumberInstance && right is NumberInstance) {
                return _engine.CreateNumber((int)((NumberInstance)left).Value & (int)((NumberInstance)right).Value);
            }

            return new InvalidOperation();
        }

        public object EvaluateBitXOrExpression(BaseValue left, BaseValue right) {
            if (left is NumberInstance && right is NumberInstance) {
                return _engine.CreateNumber((int)((NumberInstance)left).Value ^ (int)((NumberInstance)right).Value);
            }

            return new InvalidOperation();
        }

        public object EvaluateBitOrExpression(BaseValue left, BaseValue right) {
            if (left is NumberInstance && right is NumberInstance) {
                return _engine.CreateNumber((int)((NumberInstance)left).Value | (int)((NumberInstance)right).Value);
            }

            return new InvalidOperation();
        }
    }
}
