using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class Vector2Instance : VectorInstance, IValue {
        public override string Name => "Vector2";

        public Vector2Instance(SkryptEngine engine, double x, double y) : base(engine) {
            Components = new double[2];
            Components[0] = x;
            Components[1] = y;
        }

        public static SkryptObject X(SkryptEngine engine, SkryptObject self) {
            var vector = self as Vector2Instance;

            return engine.CreateNumber(vector.Components[0]);
        }

        public static SkryptObject Y(SkryptEngine engine, SkryptObject self) {
            var vector = self as Vector2Instance;

            return engine.CreateNumber(vector.Components[1]);
        }

        public SkryptObject Copy() {
            return Engine.CreateVector2(Components[0], Components[1]);
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
