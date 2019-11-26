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

        public ErrorListener (SkryptEngine engine) {
            _engine = engine;
        }

        public override void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] IToken offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e) {
            //ThrowError(recognizer, offendingSymbol.TokenIndex, line, charPositionInLine, msg, e);

            _engine.ErrorHandler.AddParseError(offendingSymbol, msg);
        }

        void IAntlrErrorListener<int>.SyntaxError(IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e) {
            //ThrowError(recognizer, offendingSymbol, line, charPositionInLine, msg, e);

            _engine.ErrorHandler.AddLexError(line, charPositionInLine, msg);
        }

        //void ThrowError(IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e) {
        //    //Console.WriteLine(offendingSymbol);

        //    //throw new ParseCanceledException($"{_engine.FileHandler.File}({line},{charPositionInLine}): {msg}");

        //    _engine.Add
        //}
    }
}
