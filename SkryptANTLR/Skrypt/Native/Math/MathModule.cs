using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class MathModule : BaseModule {
        public override string Name => "Math";

        public MathModule(Engine engine) : base(engine) {
            CreateProperty("PI", engine.CreateNumber(Math.PI));
            CreateProperty("E", engine.CreateNumber(Math.E));
        }

        public static BaseObject Abs(Engine engine, BaseObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Abs(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseObject Sign(Engine engine, BaseObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Sign(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseObject Truncate(Engine engine, BaseObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Truncate(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseObject Round (Engine engine, BaseObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Round(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseObject Floor(Engine engine, BaseObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Floor(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseObject Ceil(Engine engine, BaseObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Ceiling(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseObject Sin(Engine engine, BaseObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Sin(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseObject Sinh(Engine engine, BaseObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Sinh(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseObject Cos(Engine engine, BaseObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Cos(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseObject Cosh(Engine engine, BaseObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Cosh(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseObject Tan(Engine engine, BaseObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Tan(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseObject Tanh(Engine engine, BaseObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Tanh(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseObject Atan(Engine engine, BaseObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Atan(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseObject Atan2(Engine engine, BaseObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Atan2(arguments.GetAs<NumberInstance>(0), arguments.GetAs<NumberInstance>(1)));
        }

        public static BaseObject Log(Engine engine, BaseObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Log(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseObject Log10(Engine engine, BaseObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Log10(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseObject Max(Engine engine, BaseObject self, Arguments arguments) {
            var maxValue = default(NumberInstance);

            for (int i = 0; i < arguments.Values.Length; i++) {
                var num = arguments.GetAs<NumberInstance>(i);

                if (num.Value > maxValue.Value) maxValue = num;
            }

            return maxValue;
        }

        public static BaseObject Min(Engine engine, BaseObject self, Arguments arguments) {
            var minValue = default(NumberInstance);

            for (int i = 0; i < arguments.Values.Length; i++) {
                var num = arguments.GetAs<NumberInstance>(i);

                if (num.Value < minValue.Value) minValue = num;
            }

            return minValue;
        }
    }
}
