using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt.Runtime {
    sealed class ExpressionInterpreter {
        private readonly Engine _engine;

        public ExpressionInterpreter (Engine engine) {
            _engine = engine;
        }

        public object EvaluateNotExpression (BaseValue value) {
            if (value is BooleanInstance) {
                return !(value as BooleanInstance);
            }

            return new InvalidOperation();
        }

        public object EvaluateMinusExpression(BaseValue value) {
            if (value is NumberInstance) {
                return -(value as NumberInstance);
            }

            if (value is VectorInstance) {
                return VectorInstance.ComponentMathNumeric(_engine, value as VectorInstance, (x) => -x);
            }

            return new InvalidOperation();
        }

        public object EvaluateBitNotExpression(BaseValue value) {
            if (value is NumberInstance) {
                return ~(int)(value as NumberInstance);
            }

            return new InvalidOperation();
        }

        public object EvaluatePlusExpression (BaseValue left, BaseValue right) {
            if (left is NumberInstance && right is NumberInstance) {
                return (left as NumberInstance) + (right as NumberInstance);
            }

            if (left is StringInstance) {
                return (left as StringInstance).Value + right;
            }

            if (right is StringInstance) {
                return left + (right as StringInstance).Value;
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

        public object EvaluateIsExpression(BaseValue left, BaseValue right) {
            if (
                (typeof(BaseInstance).IsAssignableFrom(left.GetType()) || typeof(BaseType).IsAssignableFrom(left.GetType())) && 
                (typeof(BaseType).IsAssignableFrom(right.GetType()) || typeof(BaseTrait).IsAssignableFrom(right.GetType()))
                ) {
                var leftType = default(BaseType);

                if (typeof(BaseInstance).IsAssignableFrom(left.GetType())) {
                    var leftInstance = left as BaseInstance;
                    leftType = leftInstance.TypeObject;
                } else {
                    leftType = left as BaseType;
                }

                if (right is BaseType rightType) {
                    return leftType.Equals(rightType);
                } else if (right is BaseTrait rightTrait) {
                    return leftType.Traits.Contains(rightTrait);
                }
            }

            if (!typeof(BaseInstance).IsAssignableFrom(left.GetType())) 
                throw new InvalidOperationException("Expected instance on left-hand side.");
            if (!(typeof(BaseType).IsAssignableFrom(right.GetType()) || typeof(BaseTrait).IsAssignableFrom(right.GetType())))
                throw new InvalidOperationException("Expected type or trait on right-hand side.");

            return new InvalidOperation();
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
