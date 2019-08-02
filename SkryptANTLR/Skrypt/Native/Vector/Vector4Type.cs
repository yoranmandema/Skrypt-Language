using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class Vector4Type : VectorType {
        public Vector4Type(Engine engine) : base(engine) {
            CreateProperty("zero", Construct(0, 0, 0, 0));
            CreateProperty("one", Construct(1, 1, 1, 1));
            CreateProperty("positiveInfinity", Construct(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity));
            CreateProperty("negativeInfinity", Construct(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity));
        }
    }
}