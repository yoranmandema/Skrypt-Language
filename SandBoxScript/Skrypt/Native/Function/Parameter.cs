using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skrypt.ANTLR;

namespace Skrypt {
    internal class Parameter {
        public string Name { get; set; }
        public SkryptParser.ExpressionContext Default { get; set; }

        public Parameter (string name, SkryptParser.ExpressionContext def) {
            Name = name;
            Default = def;
        }
    }
}
