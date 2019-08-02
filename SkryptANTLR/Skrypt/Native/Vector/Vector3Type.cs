using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class Vector3Type : VectorType {
        public Vector3Type(Engine engine) : base(engine) {
            CreateProperty("forward", Construct(1, 0, 0));
            CreateProperty("back", Construct(-1, 0, 0));
            CreateProperty("down", Construct(0, -1, 0));
            CreateProperty("up", Construct(0, 1, 0));
            CreateProperty("right", Construct(0, 0, 1));
            CreateProperty("left", Construct(0, 0, -1));
            CreateProperty("zero", Construct(0, 0, 0));
            CreateProperty("one", Construct(1, 1, 1));
            CreateProperty("positiveInfinity", Construct(double.PositiveInfinity, double.PositiveInfinity, double.PositiveInfinity));
            CreateProperty("negativeInfinity", Construct(double.NegativeInfinity, double.NegativeInfinity, double.NegativeInfinity));
        }
    }
}