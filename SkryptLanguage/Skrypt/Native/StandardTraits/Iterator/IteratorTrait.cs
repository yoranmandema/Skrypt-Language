using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class IteratorTrait : SkryptTrait {
        public IteratorTrait(SkryptEngine engine) : base(engine) {
        }

        public static SkryptObject Current    (SkryptEngine engine, SkryptObject self, Arguments arguments) => null;
        public static SkryptObject Next       (SkryptEngine engine, SkryptObject self, Arguments arguments) => null;
        public static SkryptObject HasNext    (SkryptEngine engine, SkryptObject self, Arguments arguments) => null;
        public static SkryptObject Reset      (SkryptEngine engine, SkryptObject self, Arguments arguments) => null;
    }
}
