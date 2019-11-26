using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;

namespace Skrypt {
    public abstract class ErrorHandler {
       
        protected readonly SkryptEngine _engine;

        public List<CodeError> Errors = new List<CodeError>();
        public bool HasErrors => Errors.Any();

        public ErrorHandler (SkryptEngine engine) {
            _engine = engine;
        }

        public void AddParseError (IToken token, string message) {
            Errors.Add(new ParseError(token, message, _engine.FileHandler.File));
        }

        public void AddLexError (int line, int charInLine, string message) {
            Errors.Add(new LexError(line, charInLine, message, _engine.FileHandler.File));
        }

        public abstract void FatalError(IToken token, string msg); 
        public abstract string ReportError(CodeError error);

        public void ReportAllErrors() {
            var sorted = Errors.OrderBy(x => x.File).ThenBy(x => x.Line).ThenBy(x => x.CharInLine);

            foreach (var error in sorted) {
                ReportError(error);
            }
        }
    }
}
