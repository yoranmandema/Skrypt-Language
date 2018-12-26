using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    class InvalidTemplateTargetException : Exception {
        public InvalidTemplateTargetException() : base() { }
        public InvalidTemplateTargetException(string message) : base(message) { }
        public InvalidTemplateTargetException(string message, System.Exception inner) : base(message, inner) { }
    }
}