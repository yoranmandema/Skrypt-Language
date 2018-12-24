using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public class BinaryOperationAttribute : OperationAttribute {
        public string LeftType;
        public string RightType;

        public BinaryOperationAttribute(string op, string left, string right) : base(op) {
            LeftType = left;
            RightType = right;
        }
    }
}
