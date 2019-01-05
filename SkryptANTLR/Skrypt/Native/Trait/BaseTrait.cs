using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class BaseTrait : BaseValue {
        public BaseTrait(Engine engine) : base(engine) {
        }

        public Dictionary<string, Member> TraitMembers = new Dictionary<string, Member>();
    }
}
