using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class Vector2Type : VectorType {
        public Vector2Type(Engine engine) : base(engine) {
            CreateProperty("up", Construct(0, 1));
            CreateProperty("down", Construct(0,-1));
            CreateProperty("left", Construct(-1, 0));
            CreateProperty("right", Construct(1, 0));
            CreateProperty("one", Construct(1,1));
            CreateProperty("zero", Construct(0,0));
            CreateProperty("positiveInfinity", Construct(double.PositiveInfinity, double.PositiveInfinity));
            CreateProperty("negativeInfinity", Construct(double.NegativeInfinity, double.NegativeInfinity));
        }
    }
}