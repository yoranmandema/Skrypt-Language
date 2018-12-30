using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public class Vector3Instance : VectorInstance {
        public Vector3Instance(Engine engine, double x, double y, double z) : base(engine) {
            components = new double[3];
            components[0] = x;
            components[1] = y;
            components[2] = z;
        }

        public static BaseValue X(Engine engine, BaseValue self) {
            var vector = self as Vector3Instance;

            return engine.CreateNumber(vector.components[0]);
        }

        public static BaseValue Y(Engine engine, BaseValue self) {
            var vector = self as Vector3Instance;

            return engine.CreateNumber(vector.components[1]);
        }

        public static BaseValue Z(Engine engine, BaseValue self) {
            var vector = self as Vector3Instance;

            return engine.CreateNumber(vector.components[2]);
        }
    }
}
