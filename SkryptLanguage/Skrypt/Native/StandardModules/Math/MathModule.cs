using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class MathModule : SkryptModule {
        public override string Name => "Math";

        private readonly static Random _random = new Random();

        public MathModule(SkryptEngine engine) : base(engine) {
            CreateProperty("PI", engine.CreateNumber(Math.PI));
            CreateProperty("E", engine.CreateNumber(Math.E));
            CreateProperty("LN10", engine.CreateNumber(Math.Log(10)));
            CreateProperty("LN2", engine.CreateNumber(Math.Log(2)));
            CreateProperty("LOG2E", engine.CreateNumber(Math.Log(Math.E,2)));
            CreateProperty("LOG10E", engine.CreateNumber(Math.Log(Math.E,10)));
            CreateProperty("SQRT1_2", engine.CreateNumber(Math.Sqrt(0.5)));
            CreateProperty("SQRT2", engine.CreateNumber(Math.Sqrt(2)));
        }

        public static SkryptObject Abs(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Abs(arguments.GetAs<NumberInstance>(0)));
        }

        public static SkryptObject Sign(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Sign(arguments.GetAs<NumberInstance>(0)));
        }

        public static SkryptObject Truncate(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Truncate(arguments.GetAs<NumberInstance>(0)));
        }

        public static SkryptObject Round (SkryptEngine engine, SkryptObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Round(arguments.GetAs<NumberInstance>(0)));
        }

        public static SkryptObject Floor(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Floor(arguments.GetAs<NumberInstance>(0)));
        }

        public static SkryptObject Ceil(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Ceiling(arguments.GetAs<NumberInstance>(0)));
        }

        public static SkryptObject Sin(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Sin(arguments.GetAs<NumberInstance>(0)));
        }

        public static SkryptObject Sinh(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Sinh(arguments.GetAs<NumberInstance>(0)));
        }

        public static SkryptObject Cos(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Cos(arguments.GetAs<NumberInstance>(0)));
        }

        public static SkryptObject Cosh(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Cosh(arguments.GetAs<NumberInstance>(0)));
        }

        public static SkryptObject Tan(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Tan(arguments.GetAs<NumberInstance>(0)));
        }

        public static SkryptObject Tanh(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Tanh(arguments.GetAs<NumberInstance>(0)));
        }

        public static SkryptObject Atan(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Atan(arguments.GetAs<NumberInstance>(0)));
        }

        public static SkryptObject Atan2(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Atan2(arguments.GetAs<NumberInstance>(0), arguments.GetAs<NumberInstance>(1)));
        }

        public static SkryptObject Log(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Log(arguments.GetAs<NumberInstance>(0)));
        }

        public static SkryptObject Log2(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Log(arguments.GetAs<NumberInstance>(0),2));
        }

        public static SkryptObject Log10(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            return engine.CreateNumber(Math.Log10(arguments.GetAs<NumberInstance>(0)));
        }

        public static SkryptObject Random(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            if (arguments.Values.Length == 1) {
                return engine.CreateNumber(_random.NextDouble() * arguments.GetAs<NumberInstance>(0).Value);
            } else if (arguments.Values.Length == 2) {
                var val = _random.NextDouble();
                var min = arguments.GetAs<NumberInstance>(0).Value;
                var max= arguments.GetAs<NumberInstance>(1).Value;

                return engine.CreateNumber(max * val + min * (1 - val));
            }

            return engine.CreateNumber(_random.NextDouble());
        }

        public static SkryptObject RandomInt(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            if (arguments.Values.Length == 1) {
                return engine.CreateNumber((int)(_random.NextDouble() * arguments.GetAs<NumberInstance>(0).Value));
            }
            else if (arguments.Values.Length == 2) {
                var val = _random.NextDouble();
                var min = arguments.GetAs<NumberInstance>(0).Value;
                var max = arguments.GetAs<NumberInstance>(1).Value;

                return engine.CreateNumber((int)(max * val + min * (1 - val)));
            }

            return engine.CreateNumber((int)_random.NextDouble());
        }

        public static SkryptObject Max(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            var maxValue = default(NumberInstance);

            for (int i = 0; i < arguments.Values.Length; i++) {
                var num = arguments.GetAs<NumberInstance>(i);

                if (num.Value > maxValue.Value) maxValue = num;
            }

            return maxValue;
        }

        public static SkryptObject Min(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            var minValue = default(NumberInstance);

            for (int i = 0; i < arguments.Values.Length; i++) {
                var num = arguments.GetAs<NumberInstance>(i);

                if (num.Value < minValue.Value) minValue = num;
            }

            return minValue;
        }
    }
}
