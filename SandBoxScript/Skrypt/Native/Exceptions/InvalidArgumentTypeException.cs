using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    [Serializable]
    internal class InvalidArgumentTypeException : Exception {
        public InvalidArgumentTypeException() : base() { }
        public InvalidArgumentTypeException(string message) : base(message) { }
        public InvalidArgumentTypeException(string message, System.Exception inner) : base(message, inner) { }
    }
}