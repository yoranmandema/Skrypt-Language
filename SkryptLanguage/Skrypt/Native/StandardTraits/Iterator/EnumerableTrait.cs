using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class EnumerableTrait : SkryptTrait {
        public EnumerableTrait(SkryptEngine engine) : base(engine) {}

        public static SkryptObject GetIterator(SkryptEngine engine, SkryptObject self, Arguments arguments) => null;
        public static SkryptObject Get(SkryptEngine engine, SkryptObject self, Arguments arguments) => null;
        public static SkryptObject IsInRange(SkryptEngine engine, SkryptObject self, Arguments arguments) => null;
    }
}
