using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skrypt.ANTLR;
using Antlr4.Runtime;

namespace Skrypt {
    public class Member {
        public BaseObject value;
        public bool isPrivate;
        public bool isConstant;
        public RuleContext definitionBlock;

        public Member(BaseObject v, bool isPrivate, RuleContext block) {
            value = v;
            this.isPrivate = isPrivate;
            definitionBlock = block;
        }
    }
}
