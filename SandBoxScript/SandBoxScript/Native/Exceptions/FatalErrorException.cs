using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    [Serializable]
    public class FatalErrorException : Exception {
        public FatalErrorException() : base() { }
        public FatalErrorException(string message) : base(message) { }
        public FatalErrorException(string message, System.Exception inner) : base(message, inner) { }
    }
}
