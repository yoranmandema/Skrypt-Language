using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public class InvalidArgumentCountException : Exception {
        public InvalidArgumentCountException() : base() { }
        public InvalidArgumentCountException(string message) : base(message) { }
        public InvalidArgumentCountException(string message, System.Exception inner) : base(message, inner) { }
    }
}
