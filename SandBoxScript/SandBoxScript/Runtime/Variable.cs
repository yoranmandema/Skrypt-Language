using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public class Variable {
        public string Name { get; set; }
        public BaseValue Value { get; set; }

        public Variable (string name) {
            Name = name;
            Value = default(BaseValue);
        }
    }
}
