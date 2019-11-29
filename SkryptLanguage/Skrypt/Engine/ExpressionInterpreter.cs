using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt.Runtime {
    sealed class ExpressionInterpreter {
        private readonly SkryptEngine _engine;

        public ExpressionInterpreter (SkryptEngine engine) {
            _engine = engine;
        }

        public object EvaluateNotExpression (SkryptObject value) {
            if (value is BooleanInstance) {
                return !(value as BooleanInstance);
            }

            return new InvalidOperation();
        }

        public object EvaluateMinusExpression(SkryptObject value) {
            if (value is NumberInstance) {
                return -(value as NumberInstance);
            }

            if (value is VectorInstance) {
                return VectorInstance.ComponentMathNumeric(_engine, value as VectorInstance, (x) => -x);
            }

            return new InvalidOperation();
        }

        public object EvaluateBitNotExpression(SkryptObject value) {
            if (value is NumberInstance) {
                return ~(int)(value as NumberInstance).Value;
            }

            return new InvalidOperation();
        }

        public object EvaluatePlusExpression (SkryptObject left, SkryptObject right) {
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

        public object EvaluateSubtractExpression(SkryptObject left, SkryptObject right) {
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

        public object EvaluateMultiplyExpression(SkryptObject left, SkryptObject right) {
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

            if (left is ArrayInstance && right is NumberInstance) {
                var list = new List<SkryptObject>();

                for (int i = 0; i < (right as NumberInstance); i++) {
                    list.AddRange((left as ArrayInstance).SequenceValues);
                }

                var repeated = _engine.CreateArray(list.ToArray());
                repeated.Dictionary = (left as ArrayInstance).Dictionary;

                return repeated;
            }

            return new InvalidOperation();
        }

        public object EvaluateDivideExpression(SkryptObject left, SkryptObject right) {
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

        public object EvaluateRemainderExpression(SkryptObject left, SkryptObject right) {
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

        public object EvaluateExponentExpression(SkryptObject left, SkryptObject right) {
            if (left is NumberInstance && right is NumberInstance) {
                return Math.Pow(left as NumberInstance, right as NumberInstance);
            }

            return new InvalidOperation();
        }

        public object EvaluateLessExpression(SkryptObject left, SkryptObject right) {
            if (left is NumberInstance && right is NumberInstance) {
                return (left as NumberInstance).Value < (right as NumberInstance).Value;
            }

            return new InvalidOperation();
        }

        public object EvaluateLessEqualExpression(SkryptObject left, SkryptObject right) {
            if (left is NumberInstance && right is NumberInstance) {
                return (left as NumberInstance).Value <= (right as NumberInstance).Value;
            }

            return new InvalidOperation();
        }

        public object EvaluateGreaterExpression(SkryptObject left, SkryptObject right) {
            if (left is NumberInstance && right is NumberInstance) {
                return (left as NumberInstance).Value > (right as NumberInstance).Value;
            }

            return new InvalidOperation();
        }

        public object EvaluateGreaterEqualExpression(SkryptObject left, SkryptObject right) {
            if (left is NumberInstance && right is NumberInstance) {
                return (left as NumberInstance).Value >= (right as NumberInstance).Value;
            }

            return new InvalidOperation();
        }

        public object EvaluateEqualExpression(SkryptObject left, SkryptObject right) {
            //if (left is NumberInstance && right is NumberInstance) {
            //    return (left as NumberInstance).Value == (right as NumberInstance).Value;
            //}
            //else if (left is StringInstance && right is StringInstance) {
            //    return (left as StringInstance).Value == (right as StringInstance).Value;
            //}
            //else if (left is BooleanInstance && right is BooleanInstance) {
            //    return (left as BooleanInstance).Value == (right as BooleanInstance).Value;
            //}
            //else 
            if (left is IValue valL && right is IValue valR) {
                return valL.Equals(valR);
            }
            else {
                return left == right;
            }
        }

        public object EvaluateNotEqualExpression(SkryptObject left, SkryptObject right) {
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

        public object EvaluateIsExpression(SkryptObject left, SkryptObject right) {
            if (
                (left is SkryptInstance || left is BaseType) && 
                (right is SkryptInstance || (right is BaseType || right is BaseTrait))
                ) {

                var leftType = default(BaseType);

                if (typeof(SkryptInstance).IsAssignableFrom(left.GetType())) {
                    var leftInstance = left as SkryptInstance;
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

            if (!(left is SkryptInstance)) 
                throw new InvalidOperationException("Expected instance on left-hand side.");
            if (!(right is BaseType || right is BaseTrait))
                throw new InvalidOperationException("Expected type or trait on right-hand side.");

            return new InvalidOperation();
        }

        public object EvaluateAndExpression(SkryptObject left, SkryptObject right) {
            if (left is BooleanInstance && right is BooleanInstance) {
                return (left as BooleanInstance).Value && (right as BooleanInstance).Value;
            }

            return new InvalidOperation();
        }

        public object EvaluateOrExpression(SkryptObject left, SkryptObject right) {
            if (left is BooleanInstance && right is BooleanInstance) {
                return (left as BooleanInstance).Value || (right as BooleanInstance).Value;
            }

            return new InvalidOperation();
        }

        public object EvaluateBitAndExpression(SkryptObject left, SkryptObject right) {
            if (left is NumberInstance && right is NumberInstance) {
                return (int)(left as NumberInstance).Value & (int)(right as NumberInstance).Value;
            }

            return new InvalidOperation();
        }

        public object EvaluateBitXOrExpression(SkryptObject left, SkryptObject right) {
            if (left is NumberInstance && right is NumberInstance) {
                return (int)(left as NumberInstance).Value ^ (int)(right as NumberInstance).Value;
            }

            return new InvalidOperation();
        }

        public object EvaluateBitOrExpression(SkryptObject left, SkryptObject right) {
            if (left is NumberInstance && right is NumberInstance) {
                return (int)(left as NumberInstance).Value | (int)(right as NumberInstance).Value;
            }

            return new InvalidOperation();
        }

        public object EvaluateBitShiftLExpression(SkryptObject left, SkryptObject right) {
            if (left is NumberInstance && right is NumberInstance) {
                return (int)(left as NumberInstance).Value << (int)(right as NumberInstance).Value;
            }

            return new InvalidOperation();
        }

        public object EvaluateBitShiftRExpression(SkryptObject left, SkryptObject right) {
            if (left is NumberInstance && right is NumberInstance) {
                return (int)(left as NumberInstance).Value >> (int)(right as NumberInstance).Value;
            }

            return new InvalidOperation();
        }

        public object EvaluateBitShiftURExpression(SkryptObject left, SkryptObject right) {
            if (left is NumberInstance && right is NumberInstance) {
                return (int)((uint)Convert.ToInt32((left as NumberInstance).Value) >> Convert.ToInt32((right as NumberInstance).Value));
            }

            return new InvalidOperation();
        }
    }
}
