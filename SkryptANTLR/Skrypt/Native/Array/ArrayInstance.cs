using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class ArrayInstance : BaseInstance {
        public List<BaseValue> SequenceValues = new List<BaseValue>();
        public Dictionary<BaseValue,BaseValue> Dictionary = new Dictionary<BaseValue, BaseValue>();

        public ArrayInstance(Engine engine) : base(engine) {
        }

        public BaseValue Get (int index) {
            if (index >= 0 && index < SequenceValues.Count) {
                return SequenceValues[index];
            } else {
                return null;
            }
        }

        public BaseValue Get(BaseValue index) {
            if (index is NumberInstance number && number >= 0 && number % 1 == 0) {
                return Get((int)number);
            }

            if (Dictionary.ContainsKey(index)) {
                return Dictionary[index];
            }

            return null;
        }

        public BaseValue Set(BaseValue index, BaseValue value) {
            if (index is NumberInstance number && number >= 0 && number % 1 == 0) {
                if (number >= 0 && number < SequenceValues.Count) {
                    SequenceValues[(int)number] = value;
                }
                else if (number >= SequenceValues.Count) {
                    var diff = number - SequenceValues.Count + 1;

                    for (int i = 0; i < diff; i++) {
                        SequenceValues.Add(null);
                    }

                    SequenceValues[(int)number] = value;
                }
            }
            else {
                Dictionary[index] = value;
            }

            return this;
        }

        public static BaseValue Length(Engine engine, BaseValue self) {
            return engine.CreateNumber((self as ArrayInstance).SequenceValues.Count);
        }

        public static BaseValue Push(Engine engine, BaseValue self, Arguments arguments) {
            foreach (var a in arguments.Values) {
                (self as ArrayInstance).SequenceValues.Add(a);
            }

            return null;
        }

        public static BaseValue Insert(Engine engine, BaseValue self, Arguments arguments) {
            var array = self as ArrayInstance;
            var index = (int)Math.Min(Math.Max(Math.Round(arguments.GetAs<NumberInstance>(0)),0), array.SequenceValues.Count);

            if (arguments.Values.Length > 1) {
                for (int i = arguments.Values.Length - 1; i > 0; i--) {
                    array.SequenceValues.Insert(index, arguments.Values[i]);
                }
            }

            return null;
        }

        public override string ToString() {
            return $"[{string.Join(",", SequenceValues)}]";
        }
    }
}
