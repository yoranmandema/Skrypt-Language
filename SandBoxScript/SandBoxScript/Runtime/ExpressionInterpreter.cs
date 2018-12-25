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
            if (left.GetType() == typeof(NumberInstance) && left.GetType() == typeof(NumberInstance)) {
                return _engine.CreateNumber(
                        left.AsType<NumberInstance>().Value + right.AsType<NumberInstance>().Value
                    );
            }

            return new InvalidOperation();
        }

        public object EvaluateMultiplyExpression(BaseValue left, BaseValue right) {
            if (left.GetType() == typeof(NumberInstance) && left.GetType() == typeof(NumberInstance)) {
                return _engine.CreateNumber(
                        left.AsType<NumberInstance>().Value * right.AsType<NumberInstance>().Value
                    );
            }

            return new InvalidOperation();
        }

        public object EvaluateExponentExpression(BaseValue left, BaseValue right) {
            if (left.GetType() == typeof(NumberInstance) && left.GetType() == typeof(NumberInstance)) {
                return _engine.CreateNumber(
                        Math.Pow(left.AsType<NumberInstance>().Value,right.AsType<NumberInstance>().Value)
                    );
            }

            return new InvalidOperation();
        }
    }
}
