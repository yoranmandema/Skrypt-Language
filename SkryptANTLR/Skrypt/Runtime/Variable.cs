using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class Variable {
        public string Name { get; set; }
        public BaseValue Value { get; set; }
        public bool IsConstant { get; set; }

        public Variable (string name) {
            Name = name;
            Value = default(BaseValue);
        }

        public Variable(string name, BaseValue value) : this(name){
            Name = name;
            Value = value;
        }
    }
}
