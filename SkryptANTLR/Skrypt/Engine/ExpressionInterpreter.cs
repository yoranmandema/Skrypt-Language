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

        public object EvaluateNotExpression (BaseObject value) {
            if (value is BooleanInstance) {
                return !(value as BooleanInstance);
            }

            return new InvalidOperation();
        }

        public object EvaluateMinusExpression(BaseObject value) {
            if (value is NumberInstance) {
                return -(value as NumberInstance);
            }

            if (value is VectorInstance) {
                return VectorInstance.ComponentMathNumeric(_engine, value as VectorInstance, (x) => -x);
            }

            return new InvalidOperation();
        }

        public object EvaluateBitNotExpression(BaseObject value) {
            if (value is NumberInstance) {
                return ~(int)(value as NumberInstance).Value;
            }

            return new InvalidOperation();
        }

        public object EvaluatePlusExpression (BaseObject left, BaseObject right) {
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

        public object EvaluateSubtractExpression(BaseObject left, BaseObject right) {
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

        public object EvaluateMultiplyExpression(BaseObject left, BaseObject right) {
            if (left is NumberInstance && right is NumberInstance) {
                return (left as NumberInstance) * (right as NumberInstance);
            }

            if (left is StringInstance && right is NumberInstance) {
                var str = (left as StringInstance).Value;
                var amt = (int)(right as NumberInstance).Value;

                return new StringBuilder(str.Length * amt).Insert(0, str, amt).ToString();
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

        public object EvaluateDivideExpression(BaseObject left, BaseObject right) {
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

        public object EvaluateRemainderExpression(BaseObject left, BaseObject right) {
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

        public object EvaluateExponentExpression(BaseObject left, BaseObject right) {
            if (left is NumberInstance && right is NumberInstance) {
                return Math.Pow(left as NumberInstance, right as NumberInstance);
            }

            return new InvalidOperation();
        }

        public object EvaluateLessExpression(BaseObject left, BaseObject right) {
            if (left is NumberInstance && right is NumberInstance) {
                return (left as NumberInstance).Value < (right as NumberInstance).Value;
            }

            return new InvalidOperation();
        }

        public object EvaluateLessEqualExpression(BaseObject left, BaseObject right) {
            if (left is NumberInstance && right is NumberInstance) {
                return (left as NumberInstance).Value <= (right as NumberInstance).Value;
            }

            return new InvalidOperation();
        }

        public object EvaluateGreaterExpression(BaseObject left, BaseObject right) {
            if (left is NumberInstance && right is NumberInstance) {
                return (left as NumberInstance).Value > (right as NumberInstance).Value;
            }

            return new InvalidOperation();
        }

        public object EvaluateGreaterEqualExpression(BaseObject left, BaseObject right) {
            if (left is NumberInstance && right is NumberInstance) {
                return (left as NumberInstance).Value >= (right as NumberInstance).Value;
            }

            return new InvalidOperation();
        }

        public object EvaluateEqualExpression(BaseObject left, BaseObject right) {
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

        public object EvaluateNotEqualExpression(BaseObject left, BaseObject right) {
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

        public object EvaluateIsExpression(BaseObject left, BaseObject right) {
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

        public object EvaluateAndExpression(BaseObject left, BaseObject right) {
            if (left is BooleanInstance && right is BooleanInstance) {
                return (left as BooleanInstance).Value && (right as BooleanInstance).Value;
            }

            return new InvalidOperation();
        }

        public object EvaluateOrExpression(BaseObject left, BaseObject right) {
            if (left is BooleanInstance && right is BooleanInstance) {
                return (left as BooleanInstance).Value && (right as BooleanInstance).Value;
            }

            return new InvalidOperation();
        }

        public object EvaluateBitAndExpression(BaseObject left, BaseObject right) {
            if (left is NumberInstance && right is NumberInstance) {
                return (int)(left as NumberInstance).Value & (int)(right as NumberInstance).Value;
            }

            return new InvalidOperation();
        }

        public object EvaluateBitXOrExpression(BaseObject left, BaseObject right) {
            if (left is NumberInstance && right is NumberInstance) {
                return (int)(left as NumberInstance).Value ^ (int)(right as NumberInstance).Value;
            }

            return new InvalidOperation();
        }

        public object EvaluateBitOrExpression(BaseObject left, BaseObject right) {
            if (left is NumberInstance && right is NumberInstance) {
                return (int)(left as NumberInstance).Value | (int)(right as NumberInstance).Value;
            }

            return new InvalidOperation();
        }
    }
}
