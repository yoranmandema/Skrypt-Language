using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public interface IFunction {
        BaseValue Run(Engine engine, BaseValue self, Arguments args);
    }
}
