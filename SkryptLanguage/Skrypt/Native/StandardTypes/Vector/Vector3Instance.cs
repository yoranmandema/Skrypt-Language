using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class Vector3Instance : VectorInstance, IValue {
        public override string Name => "Vector3";

        public Vector3Instance(SkryptEngine engine, double x, double y, double z) : base(engine) {
            Components = new double[3];
            Components[0] = x;
            Components[1] = y;
            Components[2] = z;
        }

        public static SkryptObject X(SkryptEngine engine, SkryptObject self) {
            var vector = self as Vector3Instance;

            return engine.CreateNumber(vector.Components[0]);
        }

        public static SkryptObject Y(SkryptEngine engine, SkryptObject self) {
            var vector = self as Vector3Instance;

            return engine.CreateNumber(vector.Components[1]);
        }

        public static SkryptObject Z(SkryptEngine engine, SkryptObject self) {
            var vector = self as Vector3Instance;

            return engine.CreateNumber(vector.Components[2]);
        }

        public SkryptObject Copy() {
            return Engine.CreateVector3(Components[0], Components[1], Components[2]);
        }

        bool IValue.Equals(IValue other) {
            bool isEqual = true;

            for (int i = 0; i < Components.Length; i++) {
                if (this.Components[i] != ((VectorInstance)other).Components[i]) {
                    isEqual = false;
                    break;
                }
            }

            return isEqual;
        }
    }
}
