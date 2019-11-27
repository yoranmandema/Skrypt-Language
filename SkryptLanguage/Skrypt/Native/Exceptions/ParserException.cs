using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class ParserException : Exception {
        public ParseError Error { get; }

        public override string Source => Error?.Source;
        public string Description => Error?.Message;
        public string File => Error?.File;
        public int Index => Error?.Index ?? -1;
        public int LineNumber => Error?.Line ?? 0;
        public int Column => Error?.Column ?? 0;

        public ParserException() :
            this(null, null, null) { }

        public ParserException(string message) :
            this(message, null, null) { }

        public ParserException(string message, Exception innerException) :
            this(message, null, innerException) { }

        public ParserException(ParseError error) :
            this(null, error) { }

        public ParserException(string message, ParseError error) :
            this(message, error, null) { }

        public ParserException(string message, ParseError error, Exception innerException) : base(message ?? error?.ToString(), innerException) {
            Error = error;
        }
    }
}
