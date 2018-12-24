using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public class TypeConverter {
        public static NumberObject ToNumber (BaseObject val) {
            return (NumberObject)val;
        }
    }
}
