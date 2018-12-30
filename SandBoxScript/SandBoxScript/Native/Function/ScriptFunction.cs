using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SandBoxScript.ANTLR;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace SandBoxScript {
    public class ScriptFunction : IFunction {
        public IParseTree Block;

        public ScriptFunction(IParseTree block) {
            Block = block;
        }

        public BaseValue Run(Engine engine, BaseValue self, Arguments args) {

            engine.Visitor.Visit(Block);

            return null;
        }
    }
}
