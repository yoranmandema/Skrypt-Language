using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class Vector4Instance : VectorInstance, IValue {
        public override string Name => "Vector4";

        public Vector4Instance(Engine engine, double x, double y, double z, double w) : base(engine) {
            Components = new double[4];
            Components[0] = x;
            Components[1] = y;
            Components[2] = z;
            Components[3] = w;
        }

        public static BaseObject X(Engine engine, BaseObject self) {
            var vector = self as Vector4Instance;

            return engine.CreateNumber(vector.Components[0]);
        }

        public static BaseObject Y(Engine engine, BaseObject self) {
            var vector = self as Vector4Instance;

            return engine.CreateNumber(vector.Components[1]);
        }

        public static BaseObject Z(Engine engine, BaseObject self) {
            var vector = self as Vector4Instance;

            return engine.CreateNumber(vector.Components[2]);
        }

        public static BaseObject W(Engine engine, BaseObject self) {
            var vector = self as Vector4Instance;

            return engine.CreateNumber(vector.Components[2]);
        }

        public BaseObject Copy() {
            return Engine.CreateVector4(Components[0], Components[1], Components[2], Components[3]);
        }
    }
}
