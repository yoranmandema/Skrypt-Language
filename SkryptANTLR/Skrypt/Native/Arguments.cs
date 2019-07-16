using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class Arguments {
        public BaseObject[] Values;
        public int Length => Values.Length;

        public static Arguments Empty => new Arguments(new BaseObject[0]);

        public Arguments(params BaseObject[] values) {
            Values = values;
        }

        public BaseObject this[int key] {
            get {
                if (key >= 0 && key < Values.Length) {
                    return Values[key];
                } else {
                    return null;
                }
            }
        }

        public T GetAs<T> (int i) where T : BaseObject {
            var value = this[i];

            Console.WriteLine(typeof(T).Name);

            if (value != null || value is T) {
                return value as T;
            } else {
                throw new InvalidArgumentTypeException($"Expected argument of type {typeof(T).Name}.");
            }
        }

        public override string ToString() {
            return $"[{string.Join(",", Values as object[])}]";
        }
    }
}
