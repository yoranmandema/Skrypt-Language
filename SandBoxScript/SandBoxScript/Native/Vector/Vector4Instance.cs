using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public class Vector4Instance : VectorInstance {
        public Vector4Instance(Engine engine, double x, double y, double z, double w) : base(engine) {
            components = new double[4];
            components[0] = x;
            components[1] = y;
            components[2] = z;
            components[3] = w;
        }

        public static BaseValue X(Engine engine, BaseValue self) {
            var vector = self as Vector4Instance;

            return engine.CreateNumber(vector.components[0]);
        }

        public static BaseValue Y(Engine engine, BaseValue self) {
            var vector = self as Vector4Instance;

            return engine.CreateNumber(vector.components[1]);
        }

        public static BaseValue Z(Engine engine, BaseValue self) {
            var vector = self as Vector4Instance;

            return engine.CreateNumber(vector.components[2]);
        }

        public static BaseValue W(Engine engine, BaseValue self) {
            var vector = self as Vector4Instance;

            return engine.CreateNumber(vector.components[2]);
        }
    }
}
