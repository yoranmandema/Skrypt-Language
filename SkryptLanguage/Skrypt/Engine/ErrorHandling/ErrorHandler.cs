using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;

namespace Skrypt {
    public abstract class ErrorHandler {
       
        protected readonly SkryptEngine _engine;
        public string Source { get; internal set; }
        public string File { get; internal set; }

        public ErrorHandler (SkryptEngine engine) {
            _engine = engine;
        }

        public SkryptException CreateError(int index, int line, int column, string message) {
            return new SkryptException(message, new Error(index, line, column, message, Source, File));
        }

        public abstract void FatalError(int index, int line, int column, string msg);

        public void FatalError(IToken token, string msg) {
            FatalError(
                token.StartIndex,
                token.Line,
                token.Column,
                msg
            );
        }

        public abstract string ReportError(SkryptException error);
    }
}
