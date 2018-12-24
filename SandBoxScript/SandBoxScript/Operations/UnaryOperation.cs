using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public class UnaryOperation : IOperation {
        public string Name { get; set; }
        public Type Type { get; set; }

        public IFunction Function { get; set; }
    }
}
