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
                return _engine.CreateString(((StringInstance)left).Value + right.ToString());
            }

            if (right is StringInstance) {
                return _engine.CreateString(left.ToString() + ((StringInstance)right).Value);
            }

            return new InvalidOperation();
        }

        public object EvaluateMultiplyExpression(BaseValue left, BaseValue right) {
            if (left is NumberInstance && right is NumberInstance) {
                return _engine.CreateNumber((NumberInstance)left * (NumberInstance)right);
            }

            return new InvalidOperation();
        }

        public object EvaluateExponentExpression(BaseValue left, BaseValue right) {
            if (left is NumberInstance && right is NumberInstance) {
                return _engine.CreateNumber(Math.Pow((NumberInstance)left, (NumberInstance)right));
            }

            return new InvalidOperation();
        }

        public object EvaluateEqualExpression(BaseValue left, BaseValue right) {
            return new InvalidOperation();
        }
    }
}
