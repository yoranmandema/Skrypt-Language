using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public interface IErrorHandler {
        string Source { get; set; }
        string File { get; set; }
        bool HasErrors { get; set; }
        ICompileErrorHandler CompileErrorHandler { get; set; }
    }
}
