using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Skrypt.CLR {
    public class CLRTypeConverter {

        public static BaseModule CreateModuleFromType (Type type) {
            return null;
        }

        private static bool IsNumber(object value) {
            return value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal;
        }

        public static int ToInt(NumberInstance number) => (int)number.Value;

        public static double ToDouble(NumberInstance number) => number.Value;

        public static float ToFloat(NumberInstance number) => (float)number.Value;

        public static NumberInstance ToNumberInstance(Engine engine, double val) => engine.CreateNumber(val);

        public static BaseObject ConvertToSkryptObject (Engine engine, object value) {
            if (IsNumber(value)) {
                return ToNumberInstance(engine, Convert.ToDouble(value));
            }

            return null;
        }
    }
}
