using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4;
using SandBoxScript.ANTLR;
using System.Reflection;
using SandBoxScript.Runtime;

namespace SandBoxScript {
    public class Engine {
        public Scope Scope { get; internal set; } = new Scope();

        internal ExpressionInterpreter expressionInterpreter;
        internal TemplateMaker templateMaker;

        internal NumberObject Number;
        internal NumberConstructor NumberConstructor;

        internal StringObject String;
        internal StringConstructor StringConstructor;

        internal BooleanObject Boolean;
        internal BooleanConstructor BooleanConstructor;

        internal VectorObject Vector;
        internal VectorConstructor VectorConstructor;

        public Engine() {
            expressionInterpreter   = new ExpressionInterpreter(this);
            templateMaker           = new TemplateMaker(this);

            NumberConstructor       = new NumberConstructor(this);
            Number                  = new NumberObject(this);

            StringConstructor       = new StringConstructor(this);
            String                  = new StringObject(this);

            BooleanConstructor      = new BooleanConstructor(this);
            Boolean                 = new BooleanObject(this);

            VectorConstructor       = new VectorConstructor(this);
            Vector                  = new VectorObject(this);
        }

        public BaseValue Run (string code) {
            var inputStream         = new AntlrInputStream(code);
            var sandBoxScriptLexer  = new SandBoxScriptLexer(inputStream);
            sandBoxScriptLexer.AddErrorListener(new LexingErrorListener());

            var commonTokenStream   = new CommonTokenStream(sandBoxScriptLexer);
            var sandBoxScriptParser = new SandBoxScriptParser(commonTokenStream);

            sandBoxScriptParser.ErrorHandler = new BailErrorStrategy();

            var context = sandBoxScriptParser.block();
            var visitor = new SandBoxScriptVisitor(this);

            return visitor.Visit(context);
        }

        public BaseValue SetValue (string name, MethodDelegate value) {
            var val = new FunctionInstance(this, value);

            Scope.SetVariable(name, val);

            return val;
        }

        public NumberInstance CreateNumber(double value) {
            return NumberConstructor.Construct(value);
        }

        public StringInstance CreateString(string value) {
            return StringConstructor.Construct(value);
        }

        public BooleanInstance CreateBoolean(bool value) {
            return BooleanConstructor.Construct(value);
        }

        public Vector3Instance CreateVector3(double x, double y, double z) {
            return (Vector3Instance)VectorConstructor.Construct(x, y, z);
        }

        //public BooleanInstance CreateVector3(bool value) {
        //    return VectorConstructor.Construct(value);
        //}

        //public BooleanInstance CreateVector4(bool value) {
        //    return VectorConstructor.Construct(value);
        //}
    }
}
