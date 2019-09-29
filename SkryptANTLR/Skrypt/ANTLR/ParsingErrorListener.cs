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
    internal class ParsingErrorListener : BaseErrorListener {
        public override void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] IToken offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e) {
            //Console.WriteLine(offendingSymbol);

            //base.SyntaxError(recognizer, offendingSymbol, line, charPositionInLine, msg, e);

            Console.WriteLine(recognizer.GrammarFileName);

            throw new ParseCanceledException("line " + line + ":" + charPositionInLine + " " + msg);
        }
    }
}
