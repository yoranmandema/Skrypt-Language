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

        public static BaseValue Abs(Engine engine, BaseValue self, Arguments arguments) {
            return engine.CreateNumber(Math.Abs(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseValue Sign(Engine engine, BaseValue self, Arguments arguments) {
            return engine.CreateNumber(Math.Sign(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseValue Truncate(Engine engine, BaseValue self, Arguments arguments) {
            return engine.CreateNumber(Math.Truncate(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseValue Round (Engine engine, BaseValue self, Arguments arguments) {
            return engine.CreateNumber(Math.Round(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseValue Floor(Engine engine, BaseValue self, Arguments arguments) {
            return engine.CreateNumber(Math.Floor(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseValue Ceil(Engine engine, BaseValue self, Arguments arguments) {
            return engine.CreateNumber(Math.Ceiling(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseValue Sin(Engine engine, BaseValue self, Arguments arguments) {
            return engine.CreateNumber(Math.Sin(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseValue Sinh(Engine engine, BaseValue self, Arguments arguments) {
            return engine.CreateNumber(Math.Sinh(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseValue Cos(Engine engine, BaseValue self, Arguments arguments) {
            return engine.CreateNumber(Math.Cos(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseValue Cosh(Engine engine, BaseValue self, Arguments arguments) {
            return engine.CreateNumber(Math.Cosh(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseValue Tan(Engine engine, BaseValue self, Arguments arguments) {
            return engine.CreateNumber(Math.Tan(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseValue Tanh(Engine engine, BaseValue self, Arguments arguments) {
            return engine.CreateNumber(Math.Tanh(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseValue Atan(Engine engine, BaseValue self, Arguments arguments) {
            return engine.CreateNumber(Math.Atan(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseValue Atan2(Engine engine, BaseValue self, Arguments arguments) {
            return engine.CreateNumber(Math.Atan2(arguments.GetAs<NumberInstance>(0), arguments.GetAs<NumberInstance>(1)));
        }

        public static BaseValue Log(Engine engine, BaseValue self, Arguments arguments) {
            return engine.CreateNumber(Math.Log(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseValue Log10(Engine engine, BaseValue self, Arguments arguments) {
            return engine.CreateNumber(Math.Log10(arguments.GetAs<NumberInstance>(0)));
        }

        public static BaseValue Max(Engine engine, BaseValue self, Arguments arguments) {
            var maxValue = default(NumberInstance);

            for (int i = 0; i < arguments.Values.Length; i++) {
                var num = arguments.GetAs<NumberInstance>(i);

                if (num.Value > maxValue.Value) maxValue = num;
            }

            return maxValue;
        }

        public static BaseValue Min(Engine engine, BaseValue self, Arguments arguments) {
            var minValue = default(NumberInstance);

            for (int i = 0; i < arguments.Values.Length; i++) {
                var num = arguments.GetAs<NumberInstance>(i);

                if (num.Value < minValue.Value) minValue = num;
            }

            return minValue;
        }
    }
}
