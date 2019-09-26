using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;

namespace Skrypt {
    public struct Call {
        public string name;
        public int column;
        public int line;
        public string callFile;
        public string file;
        public IToken token;
    }
}
