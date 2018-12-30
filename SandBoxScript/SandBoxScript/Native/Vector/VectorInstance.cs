using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public class VectorInstance : BaseInstance {
        protected double[] components;

        public VectorInstance(Engine engine) : base(engine) { }

        public static BaseValue Length2 (Engine engine, BaseValue self) {
            double val = 0;
            var vector = self as VectorInstance;

            for (var i = 0; i < vector.components.Length; i++) {
                val += Math.Pow(vector.components[i],2);
            }

            return engine.CreateNumber(val);
        }

        public static  BaseValue Length(Engine engine, BaseValue self) {
            return engine.CreateNumber(Math.Sqrt((NumberInstance)Length2(engine, self)));
        }

        public override string ToString() {
            var str = "<";

            for (var i = 0; i < components.Length; i++) {
                str += (i > 0 ? "," : "") + components[i];
            }

            str += ">";

            return str;
        }
    }
}
