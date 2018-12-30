using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public class Vector4Instance : VectorInstance {
        public override string Name => "Vector4";

        public Vector4Instance(Engine engine, double x, double y, double z, double w) : base(engine) {
            Components = new double[4];
            Components[0] = x;
            Components[1] = y;
            Components[2] = z;
            Components[3] = w;
        }

        public static BaseValue X(Engine engine, BaseValue self) {
            var vector = self as Vector4Instance;

            return engine.CreateNumber(vector.Components[0]);
        }

        public static BaseValue Y(Engine engine, BaseValue self) {
            var vector = self as Vector4Instance;

            return engine.CreateNumber(vector.Components[1]);
        }

        public static BaseValue Z(Engine engine, BaseValue self) {
            var vector = self as Vector4Instance;

            return engine.CreateNumber(vector.Components[2]);
        }

        public static BaseValue W(Engine engine, BaseValue self) {
            var vector = self as Vector4Instance;

            return engine.CreateNumber(vector.Components[2]);
        }
    }
}
