using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public class NumberObject : BaseObject {
        public override string Name => "Number";

        public double Value { get; set; }

        public NumberObject() {}

        public NumberObject (double value) {
            Value = value;
        }

        [BinaryOperation("+", "Number", "Number")]
        public BaseObject AddNumberNumber(Engine engine, BaseObject self, BaseObject[] input) {
            var left = TypeConverter.ToNumber(input[0]);
            var right = TypeConverter.ToNumber(input[1]);

            return engine.Create<NumberObject>(left.Value + right.Value);
        }

        [BinaryOperation("*", "Number", "Number")]
        public BaseObject MulNumberNumber(Engine engine, BaseObject self, BaseObject[] input) {
            var left = TypeConverter.ToNumber(input[0]);
            var right = TypeConverter.ToNumber(input[1]);

            return engine.Create<NumberObject>(left.Value * right.Value);
        }

        [Static]
        public BaseObject Parse(Engine engine, BaseObject self, BaseObject[] input) {
            var value = double.Parse(input.ToString(), System.Globalization.CultureInfo.InvariantCulture);

            return Engine.Create<NumberObject>(value);
        }

        public override string ToString() {
            return Value.ToString();
        }
    }
}
