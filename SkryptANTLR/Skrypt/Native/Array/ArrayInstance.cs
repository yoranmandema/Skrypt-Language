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
            else {
                if (Dictionary.ContainsKey(index)) {
                    return Dictionary[index];
                }

                return null;
            }
        }

        public override string ToString() {
            return $"[{string.Join(",", SequenceValues)}]";
        }
    }
}
