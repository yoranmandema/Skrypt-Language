using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;

namespace Skrypt {
    public class ErrorHandler {
       
        private readonly Engine _engine;

        public List<CodeError> Errors = new List<CodeError>();
        public bool HasErrors => Errors.Any();

        public ErrorHandler (Engine engine) {
            _engine = engine;
        }

        public void AddParseError (IToken token, string message) {
            Errors.Add(new ParseError(token, message, _engine.FileHandler.File));
        }

        public void AddLexError (int line, int charInLine, string message) {
            Errors.Add(new LexError(line, charInLine, message, _engine.FileHandler.File));
        }

        public void FatalError (IToken token, string msg) {
            var errorMsg = ReportError(new ParseError(token, msg, _engine.FileHandler.File));

            throw new FatalErrorException(msg);
        }

        public string ReportError(CodeError error) {
            int line = -1;
            int column = -1;

            string positionString = default;

            if (error is ParseError parseError) {
                line = parseError.Token.Line;
                column = parseError.Token.Column;

                positionString = $"({line},{column})";
            } else if (error is LexError lexError) {
                line = lexError.Line;
                column = lexError.CharInline;

                positionString = $"({line},{column})";
            }

            var msg = error.Message;

            var finalMessage = $"{error.File}{positionString}: {msg}";

            Console.WriteLine(finalMessage);

            return finalMessage;
        }
        public void ReportAllErrors() {
            foreach (var error in Errors) {
                ReportError(error);
            }
        }
    }
}
