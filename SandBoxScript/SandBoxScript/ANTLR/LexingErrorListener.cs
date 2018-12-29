using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using SandBoxScript.ANTLR;
using SandBoxScript.Runtime;

namespace SandBoxScript {
    internal class LexingErrorListener : IAntlrErrorListener<int> {
        public void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] int offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e) {
            throw new NotImplementedException();
        }
    }
}
