using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript.ANTLR {
    public partial class SandBoxScriptParser {
        public Engine Engine { get; internal set; }

        public Dictionary<string, Variable> Globals = new Dictionary<string, Variable>();
    }
}
