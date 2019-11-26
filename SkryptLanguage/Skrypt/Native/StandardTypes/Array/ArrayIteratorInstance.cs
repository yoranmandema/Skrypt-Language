using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class ArrayIteratorInstance : SkryptInstance {
        public int Index = -1;
        public ArrayInstance Array;

        public ArrayIteratorInstance(SkryptEngine engine) : base(engine) {
        }
    }
}
