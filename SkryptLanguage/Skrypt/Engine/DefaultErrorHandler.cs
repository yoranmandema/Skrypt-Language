using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;

namespace Skrypt {

    public class DefaultErrorHandler : ErrorHandler {
        public DefaultErrorHandler(Engine engine) : base(engine) { }

        public override void FatalError(IToken token, string msg) {
            var errorMsg = ReportError(new ParseError(token, msg, _engine.FileHandler.File));

            throw new FatalErrorException(msg);
        }

        public override string ReportError(CodeError error) {
            string positionString = $"({error.Line},{error.CharInLine})";

            var msg = error.Message;

            var finalMessage = $"{error.File}{positionString}: {msg}";

            Console.WriteLine(finalMessage);

            return finalMessage;
        }
    }
}
