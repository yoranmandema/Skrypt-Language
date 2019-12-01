using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skrypt.ANTLR;

namespace Skrypt {
    internal interface IFunctionContext : IScopedContext {
        JumpState JumpState { get; set; }
        SkryptObject ReturnValue { get; set; }
        SkryptParser.StmntBlockContext StmntBlock { get; }
    }
}
