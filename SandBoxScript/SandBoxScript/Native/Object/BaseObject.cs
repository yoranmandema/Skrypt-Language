﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public class BaseObject {
        public virtual string Name { get; }

        public BaseObject StaticObject { get; set; }
        public Engine Engine { get; set; }
        public IOperation[] Operations { get; set; }
        public Dictionary<string, Member> Members;

        public virtual void Initialise(Engine engine, params object[] args) {
            Engine = engine;
        }
    }
}
