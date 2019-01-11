using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class Member {
        public BaseValue Value;
        public bool IsPrivate;
        public BaseValue Owner;

        public Member (BaseValue v, bool isPrivate) {
            Value = v;
            IsPrivate = isPrivate;
        }
    }
}
