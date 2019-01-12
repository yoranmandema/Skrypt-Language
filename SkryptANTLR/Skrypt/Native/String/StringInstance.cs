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

        public static BaseObject Substring (Engine engine, BaseObject self, Arguments arguments) {
            var str = self as StringInstance;
            var start = arguments.GetAs<NumberInstance>(0);
            var end = arguments[1];
            
            if (end == null) {
                return engine.CreateString(str.Value.Substring((int)start));
            } else {
                if (end is NumberInstance) {
                    var length = Math.Max(Math.Min((int)(NumberInstance)end, str.Value.Length - 1) - (int)start, 0);

                    return engine.CreateString(str.Value.Substring((int)start, length));
                } else {
                    throw new InvalidArgumentTypeException($"Expected argument of type Number.");
                }
            }
        }

        public static BaseObject StartsWith(Engine engine, BaseObject self, Arguments arguments) {
            var str = self as StringInstance;
            var input = arguments.GetAs<StringInstance>(0);

            return engine.CreateBoolean(str.Value.StartsWith(input.Value));
        }

        public static BaseObject EndsWith(Engine engine, BaseObject self, Arguments arguments) {
            var str = self as StringInstance;
            var input = arguments.GetAs<StringInstance>(0);

            return engine.CreateBoolean(str.Value.EndsWith(input.Value));
        }

        public static BaseObject Replace(Engine engine, BaseObject self, Arguments arguments) {
            var str = self as StringInstance;
            var input = arguments.GetAs<StringInstance>(0);
            var replacement = arguments.GetAs<StringInstance>(1);

            return engine.CreateString(str.Value.Replace(input.Value, replacement.Value));
        }

        public static BaseObject PadLeft(Engine engine, BaseObject self, Arguments arguments) {
            var str = (self as StringInstance).Value;
            var totalWidth = arguments.GetAs<NumberInstance>(0);
            var input = arguments.GetAs<StringInstance>(1);
            var newStr = "";

            while (newStr.Length < totalWidth) {
                newStr = input + newStr;
            }

            return engine.CreateString(newStr);
        }

        public static BaseObject PadRight(Engine engine, BaseObject self, Arguments arguments) {
            var str = (self as StringInstance).Value;
            var totalWidth = arguments.GetAs<NumberInstance>(0);
            var input = arguments.GetAs<StringInstance>(1);
            var newStr = "";

            while (newStr.Length < totalWidth) {
                newStr = newStr + input;
            }

            return engine.CreateString(newStr);
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
