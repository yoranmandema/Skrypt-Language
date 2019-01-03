using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class TypeConverter {
        public static NumberModule ToNumber (BaseModule val) {
            return (NumberModule)val;
        }
    }
}
