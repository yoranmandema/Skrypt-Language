using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public interface IGetProperty {
        BaseValue Run(Engine engine, BaseValue self); 
    }
}
