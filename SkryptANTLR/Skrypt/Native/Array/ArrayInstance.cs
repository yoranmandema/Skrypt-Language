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

        public override string ToString() {
            return $"[{string.Join(",", SequenceValues)}]";
        }
    }
}
