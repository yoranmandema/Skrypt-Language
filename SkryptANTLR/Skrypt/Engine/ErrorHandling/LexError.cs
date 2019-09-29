using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;

namespace Skrypt {
    public class LexError : CodeError {
        public IToken Token { get; private set; }

        public LexError(int line, int charInLine, string message, string file) : base(message, file) {
            Line = line;
            CharInLine = charInLine;
            Message = message;
        }
    }
}
