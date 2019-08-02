using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class Vector3Instance : VectorInstance, IValue {
        public override string Name => "Vector3";

        public Vector3Instance(Engine engine, double x, double y, double z) : base(engine) {
            Components = new double[3];
            Components[0] = x;
            Components[1] = y;
            Components[2] = z;
        }

        public static BaseObject X(Engine engine, BaseObject self) {
            var vector = self as Vector3Instance;

            return engine.CreateNumber(vector.Components[0]);
        }

        public static BaseObject Y(Engine engine, BaseObject self) {
            var vector = self as Vector3Instance;

            return engine.CreateNumber(vector.Components[1]);
        }

        public static BaseObject Z(Engine engine, BaseObject self) {
            var vector = self as Vector3Instance;

            return engine.CreateNumber(vector.Components[2]);
        }

        public BaseObject Copy() {
            return Engine.CreateVector3(Components[0], Components[1], Components[2]);
        }
    }
}
