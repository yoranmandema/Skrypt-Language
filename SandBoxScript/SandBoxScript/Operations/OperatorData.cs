using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    internal class OperatorData {
        public static Dictionary<string, Tuple<string,int>> Pairs = new Dictionary<string, Tuple<string, int>> {
            {"__add",  new Tuple<string, int>("+", 2) },
        };
    }
}
