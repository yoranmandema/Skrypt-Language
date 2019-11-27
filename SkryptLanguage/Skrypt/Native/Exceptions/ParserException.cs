using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class ParserException : SkryptException {
        public ParserException() : this(null, null, null) { }

        public ParserException(string message) :
            this(message, null, null) { }

        public ParserException(string message, Exception innerException) :
            this(message, null, innerException) { }

        public ParserException(Error error) :
            this(null, error) { }

        public ParserException(string message, Error error) :
            this(message, error, null) { }

        public ParserException(string message, Error error, Exception innerException) : 
            base(message ?? error?.ToString(), innerException) {}
    }
}
