using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public class NumberObject : BaseObject {
        public NumberObject (Engine engine) : base (engine) { }

        public static BaseValue Parse(Engine engine, BaseValue self, BaseValue[] input) {
            var value = double.Parse(input.ToString(), System.Globalization.CultureInfo.InvariantCulture);

            return engine.CreateNumber(value);
        }
    }
}
