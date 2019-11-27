using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;

namespace Skrypt {

    public class DefaultErrorHandler : ErrorHandler {
        public DefaultErrorHandler(SkryptEngine engine) : base(engine) { }

        public override void FatalError(int index, int line, int column, string msg) {
            throw CreateError(index, line, column, msg);
        }

        public override string ReportError(SkryptException error) {
            string positionString = $"({error.Line},{error.Column})";
            string fileAndPosition = $"{error.File}{positionString}: ";

            var finalMessage = (error.Index > -1 ? fileAndPosition : "") + $"{error.Message}";

            Console.WriteLine(finalMessage);

            return finalMessage;
        }
    }
}
