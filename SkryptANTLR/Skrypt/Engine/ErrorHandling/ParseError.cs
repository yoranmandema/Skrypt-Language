using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;

namespace Skrypt {
    public class ParseError : CodeError {
        public IToken Token { get; private set; }

        public ParseError(IToken token, string message, string file) : base (message, file) {
            Token = token;
            Message = message;
            Line = token.Line;
            CharInLine = token.Column;
        }
    }
}
