﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public class FunctionObject : BaseObject {
        public override string Name => "Function";

        public IFunction Function { get; set; }
    }
}
