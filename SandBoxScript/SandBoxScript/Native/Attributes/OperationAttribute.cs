using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public class OperationAttribute : Attribute {
        public string Name;

        public OperationAttribute (string op) {
            Name = op;
        }
    }
}
