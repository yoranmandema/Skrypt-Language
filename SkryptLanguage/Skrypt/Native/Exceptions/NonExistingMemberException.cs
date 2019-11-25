using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    [Serializable]
    internal class NonExistingMemberException : Exception {
        public NonExistingMemberException() : base() { }
        public NonExistingMemberException(string message) : base(message) { }
        public NonExistingMemberException(string message, System.Exception inner) : base(message, inner) { }
    }
}