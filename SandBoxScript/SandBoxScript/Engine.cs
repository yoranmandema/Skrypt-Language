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

        internal SandBoxScriptParser Parser;
        internal SandBoxScriptVisitor Visitor;

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

            Parser = new SandBoxScriptParser(commonTokenStream) {
                ErrorHandler = new BailErrorStrategy()
            }
            ;

            var context = Parser.program();
            Visitor = new SandBoxScriptVisitor(this);

            return Visitor.Visit(context);
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

        public Vector2Instance CreateVector2(double x, double y) {
            return (Vector2Instance)VectorConstructor.Construct(x, y);
        }

        public Vector3Instance CreateVector3(double x, double y, double z) {
            return (Vector3Instance)VectorConstructor.Construct(x, y, z);
        }

        public Vector4Instance CreateVector4(double x, double y, double z, double w) {
            return (Vector4Instance)VectorConstructor.Construct(x, y, z, w);
        }
    }
}
