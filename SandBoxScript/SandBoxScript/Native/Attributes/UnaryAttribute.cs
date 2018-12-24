using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public class UnaryOperationAttribute : OperationAttribute {
        public string Type;

        public UnaryOperationAttribute (string op, string type) : base(op) {
            Type = type;
        }
    }
}
