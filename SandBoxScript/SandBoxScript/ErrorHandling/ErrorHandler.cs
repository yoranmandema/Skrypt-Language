using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;

namespace SandBoxScript {
    public class ErrorHandler {
        public List<CodeError> Errors = new List<CodeError>();
        public bool HasErrors => Errors.Any();

        public void AddError (IToken token, string message) {
            Errors.Add(new CodeError(token, message));
        }

        public void FatalError (IToken token, string msg) {
            var errorMsg = ReportError(new CodeError(token, msg));

            throw new FatalErrorException(msg);
        }

        public void FatalError(int line, int column, string msg) {
            var errorMsg = ReportError(line, column, msg);

            throw new FatalErrorException(msg);
        }

        public string ReportError (int line, int column, string msg) {
            var message = $"Error at ({line},{column}): {msg}";

            Console.WriteLine(message);

            return message;
        }

        public string ReportError(CodeError error) {
            var line = error.Token.Line;
            var column = error.Token.Column;
            var msg = error.Message;

            return ReportError(line,column,msg);
        }
        public void ReportAllErrors() {
            foreach (var error in Errors) {
                ReportError(error);
            }
        }
    }
}
