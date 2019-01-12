using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class StringInstance : BaseInstance, IValue {
        public override string Name => "String";

        public string Value { get; set; }

        public StringInstance(Engine engine, string value) : base(engine) {
            Value = value;
        }

        public BaseObject Get(int index) {
            if (index >= 0 && index < Value.Length) {
                return Engine.CreateString(Value[index].ToString());
            }

            return Engine.CreateString(string.Empty);
        }

        public BaseObject Get(BaseObject index) {
            if (index is NumberInstance number && number >= 0 && number % 1 == 0) {
                return Get((int)number);
            }

            return Engine.CreateString(string.Empty);
        }

        public static BaseObject Length(Engine engine, BaseObject self) {
            return engine.CreateNumber((self as StringInstance).Value.Length);
        }

        public static implicit operator string(StringInstance s) {
            return s.Value;
        }

        public override string ToString() {
            return Value.ToString();
        }

        public BaseObject Copy() {
            return Engine.CreateString(Value);
        }
    }
}
