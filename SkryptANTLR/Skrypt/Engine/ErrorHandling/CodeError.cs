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
        public string Message { get; protected set; }
        public string File { get; protected set; }

        public int Line { get; protected set; }
        public int CharInLine { get; protected set; }

    public CodeError (string message, string file) {
            Message = message;
            File = file;
        }
    }
}
