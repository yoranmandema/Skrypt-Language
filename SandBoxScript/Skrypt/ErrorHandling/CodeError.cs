using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4;
using Skrypt.ANTLR;
using Skrypt.Runtime;

namespace Skrypt {
    public class CodeError {
        public readonly IToken Token;
        public readonly string Message;

        public CodeError(IToken token, string message) {
            Token = token;
            Message = message;
        }
    }
}
