using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class BaseInstance : BaseValue {
        public BaseInstance(Engine engine) : base (engine) { }
        public BaseType TypeObject { get; set; }
    }
}
