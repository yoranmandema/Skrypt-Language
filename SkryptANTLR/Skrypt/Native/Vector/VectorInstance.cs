using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class VectorInstance : BaseInstance {
        internal double[] Components;

        public VectorInstance(Engine engine) : base(engine) { }

        public static BaseObject Length2 (Engine engine, BaseObject self) {
            double val = 0;
            var vector = self as VectorInstance;

            for (var i = 0; i < vector.Components.Length; i++) {
                val += Math.Pow(vector.Components[i],2);
            }

            return engine.CreateNumber(val);
        }

        public static BaseObject Length(Engine engine, BaseObject self) {
            return engine.CreateNumber(Math.Sqrt((NumberInstance)Length2(engine, self)));
        }

        public static BaseObject Normalized(Engine engine, BaseObject self) {
            var length = (NumberInstance)Length(engine, self);

            return ComponentMathNumeric(engine, self as VectorInstance, (x) => x / length) as VectorInstance;
        }

        internal static object ComponentMath (Engine engine, VectorInstance left, VectorInstance right, Func<double, double, double> func) {
            var dimension = left.Components.Length;

            if (dimension == right.Components.Length) {
                var args = new double[dimension];

                for (var i = 0; i < dimension; i++) {
                    var newVal = func(left.Components[i],right.Components[i]);

                    args[i] = newVal;
                }

                return engine.Vector.Construct(args);
            }

            return new InvalidOperation();
        }

        internal static object ComponentMathNumeric(Engine engine, VectorInstance left, Func<double, double> func) {
            var dimension = left.Components.Length;

            var args = new double[dimension];

            for (var i = 0; i < dimension; i++) {
                var newVal = func(left.Components[i]);

                args[i] = newVal;
            }

            return engine.Vector.Construct(args);
        }

        public override string ToString() {
            var str = "<";

            for (var i = 0; i < Components.Length; i++) {
                str += (i > 0 ? "," : "") + Components[i];
            }

            str += ">";

            return str;
        }
    }
}
