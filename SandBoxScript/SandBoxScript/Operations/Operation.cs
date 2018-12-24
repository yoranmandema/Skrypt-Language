using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public interface IOperation {
        string Name { get; set; }
        IFunction Function { get; set; }
    }
}
