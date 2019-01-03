using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class Arguments {
        public BaseValue[] Values;

        public Arguments(BaseValue[] values) {
            Values = values;
        }

        public BaseValue this[int key] {
            get {
                if (key >= 0 && key < Values.Length) {
                    return Values[key];
                } else {
                    return null;
                }
            }
        }

        public T GetAs<T> (int i) where T : BaseValue {
            var value = this[i];

            if (value is T) {
                return value as T;
            } else {
                throw new InvalidArgumentTypeException($"Expected argument with type {value.GetType()}, expected {typeof(T).Name}");
            }
        }
    }
}
