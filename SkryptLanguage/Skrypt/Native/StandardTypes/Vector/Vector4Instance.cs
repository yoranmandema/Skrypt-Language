using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class Vector4Instance : VectorInstance, IValue {
        public override string Name => "Vector4";

        public Vector4Instance(SkryptEngine engine, double x, double y, double z, double w) : base(engine) {
            Components = new double[4];
            Components[0] = x;
            Components[1] = y;
            Components[2] = z;
            Components[3] = w;
        }

        public static SkryptObject X(SkryptEngine engine, SkryptObject self) {
            var vector = self as Vector4Instance;

            return engine.CreateNumber(vector.Components[0]);
        }

        public static SkryptObject Y(SkryptEngine engine, SkryptObject self) {
            var vector = self as Vector4Instance;

            return engine.CreateNumber(vector.Components[1]);
        }

        public static SkryptObject Z(SkryptEngine engine, SkryptObject self) {
            var vector = self as Vector4Instance;

            return engine.CreateNumber(vector.Components[2]);
        }

        public static SkryptObject W(SkryptEngine engine, SkryptObject self) {
            var vector = self as Vector4Instance;

            return engine.CreateNumber(vector.Components[2]);
        }

        public SkryptObject Copy() {
            return Engine.CreateVector4(Components[0], Components[1], Components[2], Components[3]);
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
