﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public class NumberInstance : BaseInstance {
        public override string Name => "Number";

        public double Value { get; set; }

        public NumberInstance(Engine engine, double value) : base(engine) {
            Value = value;
        }

        public static BaseValue SomeFunction (Engine engine, BaseValue self, BaseValue[] arguments) {
            return null;
        }

        public override string ToString() {
            return Value.ToString();
        }
    }
}