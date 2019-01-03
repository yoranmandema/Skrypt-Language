using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class Template {
        public string Name;
        public Dictionary<string, Member> Members = new Dictionary<string, Member>();
    }
}
