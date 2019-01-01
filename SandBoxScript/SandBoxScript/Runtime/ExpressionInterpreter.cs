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
                return (left as NumberInstance) + (right as NumberInstance);
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
                return (left as NumberInstance) - (right as NumberInstance);
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
                return (left as NumberInstance) * (right as NumberInstance);
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
                return (left as NumberInstance) / (right as NumberInstance);
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
                return (left as NumberInstance) % (right as NumberInstance);
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
                return Math.Pow(left as NumberInstance, right as NumberInstance);
            }

            return new InvalidOperation();
        }

        public object EvaluateLessExpression(BaseValue left, BaseValue right) {
            if (left is NumberInstance && right is NumberInstance) {
                return (left as NumberInstance).Value < (right as NumberInstance).Value;
            }

            return new InvalidOperation();
        }

        public object EvaluateLessEqualExpression(BaseValue left, BaseValue right) {
            if (left is NumberInstance && right is NumberInstance) {
                return (left as NumberInstance).Value <= (right as NumberInstance).Value;
            }

            return new InvalidOperation();
        }

        public object EvaluateGreaterExpression(BaseValue left, BaseValue right) {
            if (left is NumberInstance && right is NumberInstance) {
                return (left as NumberInstance).Value > (right as NumberInstance).Value;
            }

            return new InvalidOperation();
        }

        public object EvaluateGreaterEqualExpression(BaseValue left, BaseValue right) {
            if (left is NumberInstance && right is NumberInstance) {
                return (left as NumberInstance).Value >= (right as NumberInstance).Value;
            }

            return new InvalidOperation();
        }

        public object EvaluateEqualExpression(BaseValue left, BaseValue right) {
            if (left is NumberInstance && right is NumberInstance) {
                return (left as NumberInstance).Value == (right as NumberInstance).Value;
            }
            else if (left is StringInstance && right is StringInstance) {
                return (left as StringInstance).Value == (right as StringInstance).Value;
            }
            else if (left is BooleanInstance && right is BooleanInstance) {
                return (left as BooleanInstance).Value == (right as BooleanInstance).Value;
            }
            else {
                return left == right;
            }
        }

        public object EvaluateNotEqualExpression(BaseValue left, BaseValue right) {
            if (left is NumberInstance && right is NumberInstance) {
                return (left as NumberInstance).Value != (right as NumberInstance).Value;
            }
            else if (left is StringInstance && right is StringInstance) {
                return (left as StringInstance).Value != (right as StringInstance).Value;
            }
            else if (left is BooleanInstance && right is BooleanInstance) {
                return (left as BooleanInstance).Value != (right as BooleanInstance).Value;
            }
            else {
                return left != right;
            }
        }

        public object EvaluateAndExpression(BaseValue left, BaseValue right) {
            if (left is BooleanInstance && right is BooleanInstance) {
                return (left as BooleanInstance).Value && (right as BooleanInstance).Value;
            }

            return new InvalidOperation();
        }

        public object EvaluateOrExpression(BaseValue left, BaseValue right) {
            if (left is BooleanInstance && right is BooleanInstance) {
                return (left as BooleanInstance).Value && (right as BooleanInstance).Value;
            }

            return new InvalidOperation();
        }

        public object EvaluateBitAndExpression(BaseValue left, BaseValue right) {
            if (left is NumberInstance && right is NumberInstance) {
                return (int)(left as NumberInstance).Value & (int)(right as NumberInstance).Value;
            }

            return new InvalidOperation();
        }

        public object EvaluateBitXOrExpression(BaseValue left, BaseValue right) {
            if (left is NumberInstance && right is NumberInstance) {
                return (int)(left as NumberInstance).Value ^ (int)(right as NumberInstance).Value;
            }

            return new InvalidOperation();
        }

        public object EvaluateBitOrExpression(BaseValue left, BaseValue right) {
            if (left is NumberInstance && right is NumberInstance) {
                return (int)(left as NumberInstance).Value | (int)(right as NumberInstance).Value;
            }

            return new InvalidOperation();
        }
    }
}
