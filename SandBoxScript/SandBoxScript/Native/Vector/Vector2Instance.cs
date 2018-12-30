using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    public class Vector2Instance : VectorInstance {
        public Vector2Instance(Engine engine, double x, double y) : base(engine) {
            components = new double[2];
            components[0] = x;
            components[1] = y;
        }

        public static BaseValue X(Engine engine, BaseValue self) {
            var vector = self as Vector2Instance;

            return engine.CreateNumber(vector.components[0]);
        }

        public static BaseValue Y(Engine engine, BaseValue self) {
            var vector = self as Vector2Instance;

            return engine.CreateNumber(vector.components[1]);
        }
    }
}
