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
    public class ParseError {
        public string Message { get; protected set; }
        public string File { get; protected set; }
        public int Line { get; protected set; }
        public int Column { get; protected set; }
        public int Index { get; protected set; }

        public string Source { get; protected set; }

        public ParseError (string message, string source, string file) {
            Message = message;
            File = file;
            Source = source;
        }

        public ParseError(IToken token, string message, string source, string file) : this(message, source, file) {
            Index = token.StartIndex;
            Message = message;
            Line = token.Line;
            Column = token.Column;
        }

        public ParseError(int index, int line, int column, string message, string source, string file) : this(message, source, file) {
            Index = index;
            Line = line;
            Column = column;
            Message = message;
        }
    }
}
