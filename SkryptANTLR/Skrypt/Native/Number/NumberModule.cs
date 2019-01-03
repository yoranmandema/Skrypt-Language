using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class NumberModule : BaseModule {
        public NumberModule(Engine engine) : base(engine) { }

        public static BaseValue Parse(Engine engine, BaseValue self, Arguments input) {
            var value = double.Parse(input.ToString(), System.Globalization.CultureInfo.InvariantCulture);

            return engine.CreateNumber(value);
        }    
    }
}
