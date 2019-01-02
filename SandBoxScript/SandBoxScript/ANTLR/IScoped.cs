using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SandBoxScript.ANTLR;
using Antlr4.Runtime;

namespace SandBoxScript {
    internal interface IScoped {
        Dictionary<string, Variable> Variables { get; set; }
        RuleContext Context { get; }
    }
}
