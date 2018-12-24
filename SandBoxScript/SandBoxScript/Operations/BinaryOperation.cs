using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public class BinaryOperation : IOperation {   
        public string Name { get; set; }

        public Type LeftType { get; set; }
        public Type RightType { get; set; }

        public IFunction Function { get; set; }
    }
}
