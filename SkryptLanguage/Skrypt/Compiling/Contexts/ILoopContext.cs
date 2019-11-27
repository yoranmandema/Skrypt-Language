using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    internal interface ILoopContext {
        JumpState JumpState { get; set; }
    }
}
