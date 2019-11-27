using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class Variable {
        public string Name { get; set; }
        public SkryptObject Value { get; set; }
        public bool IsConstant { get; set; }

        public LexicalEnvironment LexicalEnvironment { get; set; }

        public Variable (string name) {
            Name = name;
            Value = default(SkryptObject);
        }

        public Variable (string name, LexicalEnvironment lexicalEnvironment) : this(name) {
            LexicalEnvironment = lexicalEnvironment;
        }

        public Variable(string name, SkryptObject value) : this(name) {
            Value = value;
        }

        public Variable(string name, SkryptObject value, LexicalEnvironment lexicalEnvironment) : this(name, value) {
            LexicalEnvironment = lexicalEnvironment;
        }
    }
}
