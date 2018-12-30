using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public class Vector3Instance : VectorInstance {
        public override string Name => "Vector3";

        public Vector3Instance(Engine engine, double x, double y, double z) : base(engine) {
            Components = new double[3];
            Components[0] = x;
            Components[1] = y;
            Components[2] = z;
        }

        public static BaseValue X(Engine engine, BaseValue self) {
            var vector = self as Vector3Instance;

            return engine.CreateNumber(vector.Components[0]);
        }

        public static BaseValue Y(Engine engine, BaseValue self) {
            var vector = self as Vector3Instance;

            return engine.CreateNumber(vector.Components[1]);
        }

        public static BaseValue Z(Engine engine, BaseValue self) {
            var vector = self as Vector3Instance;

            return engine.CreateNumber(vector.Components[2]);
        }
    }
}
