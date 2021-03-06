﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class SkryptInstance : SkryptObject {
        public SkryptInstance(SkryptEngine engine) : base(engine) { }
        public SkryptType TypeObject { get; set; }

        public static SkryptObject Type(SkryptEngine engine, SkryptObject self) {
            return (self as SkryptInstance).TypeObject;
        }

        public bool HasTrait<T> () where T : SkryptTrait {
            return TypeObject.Traits.OfType<T>().Any();
        }
    }
}
