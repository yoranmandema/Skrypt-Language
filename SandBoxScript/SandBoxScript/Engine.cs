using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4;
using SandBoxScript.ANTLR;
using System.Reflection;
using SandBoxScript.MethodInfoExtensions;
using SandBoxScript.Runtime;

namespace SandBoxScript {
    public class Engine {
        internal Scope Scope = new Scope();
        internal ExpressionInterpreter expressionInterpreter;
        internal TemplateMaker templateMaker;

        internal NumberObject Number;
        internal NumberConstructor NumberConstructor;

        public Engine() {
            expressionInterpreter   = new ExpressionInterpreter(this);
            templateMaker           = new TemplateMaker(this);

            NumberConstructor       = new NumberConstructor(this);
            Number                  = new NumberObject(this);          
        }

        public BaseValue Run (string code) {
            var inputStream         = new AntlrInputStream(code);
            var sandBoxScriptLexer  = new SandBoxScriptLexer(inputStream);
            var commonTokenStream   = new CommonTokenStream(sandBoxScriptLexer);
            var sandBoxScriptParser = new SandBoxScriptParser(commonTokenStream);

            var expressionContext = sandBoxScriptParser.expression();
            var visitor = new SandBoxScriptVisitor(this);

            return visitor.Visit(expressionContext);
        }

        public NumberInstance CreateNumber(double value) {
            return NumberConstructor.Construct(value);
        }
    }
}
