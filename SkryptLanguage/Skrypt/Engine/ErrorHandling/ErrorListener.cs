using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;
using Skrypt.Runtime;

namespace Skrypt {
    internal class ErrorListener : BaseErrorListener, IAntlrErrorListener<int>{

        private readonly SkryptEngine _engine;
        private readonly CompileErrorHandler _compileErrorHandler;

        public ErrorListener (SkryptEngine engine, CompileErrorHandler compileErrorHandler) {
            _engine = engine;
            _compileErrorHandler = compileErrorHandler;
        }

        public override void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] IToken offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e) {
            _compileErrorHandler.TolerateError(offendingSymbol, msg);
        }

        void IAntlrErrorListener<int>.SyntaxError(IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e) {
            _compileErrorHandler.TolerateError(e.OffendingToken, msg);
        }
    }
}
