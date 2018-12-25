using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    interface IConstructor<T> {
        T Construct(BaseValue[] arguments);
    }
}
