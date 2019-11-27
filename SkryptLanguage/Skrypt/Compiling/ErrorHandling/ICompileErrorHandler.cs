using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public interface ICompileErrorHandler {
        IList<ParserException> Errors { get; set; }
        string Source { get; set; }
        bool Tolerant { get; set; }
        void RecordError(ParserException error);
        void Tolerate(ParserException error);
        ParserException CreateError(int index, int line, int column, string message);
        void TolerateError(int index, int line, int column, string message);
    }
}
