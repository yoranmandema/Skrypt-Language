using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public interface IFunction {
        BaseObject Run(Engine engine, BaseObject self, Arguments args);
    }
}
