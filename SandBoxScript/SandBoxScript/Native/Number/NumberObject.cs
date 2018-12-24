using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public class NumberObject : BaseObject {
        public override string Name => "Number";

        public double Value { get; set; }

        public NumberObject (double value) {
            Value = value;
        }

        public NumberObject __add(NumberObject left, NumberObject right) {
            return Engine.Create<NumberObject>(left.Value + right.Value);
        }
    }
}
