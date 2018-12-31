using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SandBoxScript.ANTLR;

namespace SandBoxScript {
    internal class Parameter {
        public string Name { get; set; }
        public SandBoxScriptParser.ExpressionContext Default { get; set; }

        public Parameter (string name, SandBoxScriptParser.ExpressionContext def) {
            Name = name;
            Default = def;
        }
    }
}
