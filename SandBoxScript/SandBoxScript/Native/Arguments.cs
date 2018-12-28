using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public class Arguments {
        public BaseValue[] Values;

        public Arguments(BaseValue[] values) {
            Values = values;
        }

        public BaseValue this[int key] {
            get {
                if (key > 0 && key < Values.Length) {
                    return Values[key];
                } else {
                    return null;
                }
            }
        }
    }
}
