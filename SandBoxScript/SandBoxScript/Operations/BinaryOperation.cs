using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public class BinaryOperation : IOperation {   
        public string Name { get; set; }

        public string LeftType { get; set; }
        public string RightType { get; set; }

        public IFunction Function { get; set; }
    }
}
