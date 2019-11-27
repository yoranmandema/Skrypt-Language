using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class CompileErrorHandler : ICompileErrorHandler {
        protected readonly SkryptEngine _engine;

        public IList<ParserException> Errors = new List<ParserException>();
        public bool HasErrors => Errors.Any();
        public string Source { get; set; }
        public string File { get; set; }
        public bool Tolerant { get; set; }

        public CompileErrorHandler(SkryptEngine engine, string source) {
            _engine = engine;
            Source = source;
        }

        public void RecordError(ParserException error) {
            Errors.Add(error);
        }

        public void Tolerate(ParserException error) {
            if (Tolerant) {
                RecordError(error);
            }
            else {
                throw error;
            }
        }

        public ParserException CreateError(int index, int line, int column, string message) {
            return new ParserException(new ParseError(index, line, column, message, Source, File));
        }

        public void TolerateError (IToken token, string message) {
            TolerateError(
                token.StartIndex,
                token.Line,
                token.Column,
                "Trait expected."
            );
        }

        public void TolerateError(int index, int line, int column, string message) {
            var error = this.CreateError(index, line, column, message);
            if (Tolerant) {
                this.RecordError(error);
            }
            else {
                throw error;
            }
        }
    }
}
