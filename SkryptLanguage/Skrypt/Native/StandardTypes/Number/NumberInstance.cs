using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class NumberInstance : BaseInstance, IValue {
        public override string Name => "Number";

        public double Value { get; set; }

        public NumberInstance(Engine engine, double value) : base(engine) {
            Value = value;
        }

        public static implicit operator double(NumberInstance d) {
            return d.Value;
        }

        public override bool IsTrue() {
            return Value != 0;
        }

        public override string ToString() {
            return Value.ToString();
        }

        public BaseObject Copy() {
            return Engine.CreateNumber(Value);
        }

        bool IValue.Equals(IValue other) {
            return this.Value.Equals(((NumberInstance)other).Value);
        }
    }
}