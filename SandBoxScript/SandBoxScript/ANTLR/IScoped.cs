using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SandBoxScript.ANTLR;

namespace SandBoxScript {
    internal interface IScoped {
        Dictionary<string, Variable> Variables { get; set; }
    }
}
