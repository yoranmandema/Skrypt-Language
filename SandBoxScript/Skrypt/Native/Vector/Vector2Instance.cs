using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class Vector2Instance : VectorInstance {
        public override string Name => "Vector2";

        public Vector2Instance(Engine engine, double x, double y) : base(engine) {
            Components = new double[2];
            Components[0] = x;
            Components[1] = y;
        }

        public static BaseValue X(Engine engine, BaseValue self) {
            var vector = self as Vector2Instance;

            return engine.CreateNumber(vector.Components[0]);
        }

        public static BaseValue Y(Engine engine, BaseValue self) {
            var vector = self as Vector2Instance;

            return engine.CreateNumber(vector.Components[1]);
        }
    }
}
