using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    internal class VariableNotFoundException : Exception {
        public VariableNotFoundException() : base() { }
        public VariableNotFoundException(string message) : base(message) { }
        public VariableNotFoundException(string message, System.Exception inner) : base(message, inner) { }
    }
}
