using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    [Serializable]
    public class InvalidOperationException : Exception {
        public InvalidOperationException() : base() { }
        public InvalidOperationException(string message) : base(message) { }
        public InvalidOperationException(string message, System.Exception inner) : base(message, inner) { }
    }
}
