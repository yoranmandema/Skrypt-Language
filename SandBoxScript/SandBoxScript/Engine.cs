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
        //public Scope Scope { get; internal set; } = new Scope();

        internal SandBoxScriptParser Parser;
        internal SandBoxScriptVisitor Visitor;

        internal ExpressionInterpreter expressionInterpreter;
        internal TemplateMaker templateMaker;

        internal NumberModule Number;
        internal NumberConstructor NumberConstructor;

        internal StringModule String;
        internal StringConstructor StringConstructor;

        internal BooleanModule Boolean;
        internal BooleanConstructor BooleanConstructor;

        internal VectorModule Vector;
        internal VectorConstructor VectorConstructor;

        internal MathModule Math;

        internal SandBoxScriptParser.ProgramContext ProgramContext;
        internal Dictionary<string, Variable> Globals = new Dictionary<string, Variable>();

        public Engine() {
            expressionInterpreter   = new ExpressionInterpreter(this);
            templateMaker           = new TemplateMaker(this);

            NumberConstructor       = new NumberConstructor(this);
            Number                  = new NumberModule(this);

            StringConstructor       = new StringConstructor(this);
            String                  = new StringModule(this);

            BooleanConstructor      = new BooleanConstructor(this);
            Boolean                 = new BooleanModule(this);

            VectorConstructor       = new VectorConstructor(this);
            Vector                  = new VectorModule(this);

            Math                    = new MathModule(this);

            Visitor = new SandBoxScriptVisitor(this);
        }

        public Engine Run(string code) {
            var inputStream = new AntlrInputStream(code);
            var sandBoxScriptLexer = new SandBoxScriptLexer(inputStream);
            sandBoxScriptLexer.AddErrorListener(new LexingErrorListener());

            var commonTokenStream = new CommonTokenStream(sandBoxScriptLexer);

            Parser = new SandBoxScriptParser(commonTokenStream) {
                Engine = this,
                ErrorHandler = new BailErrorStrategy()
            }
            ;

            Parser.Globals = Globals;

            ProgramContext = Parser.program();

            Visitor.Visit(ProgramContext);

            return this;
        }

        internal void SetGlobal (string name, BaseValue value) {
            if (Globals.ContainsKey(name)) {
                Globals[name].Value = value;
            } else {
                Globals[name] = new Variable(name, value);
            }
        }

        public BaseValue SetValue(string name, BaseValue value) {
            SetGlobal(name, value);

            return value;
        }

        public BaseValue SetValue(string name, bool value) {
            var val = CreateBoolean(value);

            SetGlobal(name, val);

            return val;
        }

        public BaseValue SetValue (string name, MethodDelegate value) {
            var val = new FunctionInstance(this, value);

            SetGlobal(name,val);

            return val;
        }

        public BaseValue GetValue(string name) {
            var block = ProgramContext.block();

            if (block.Variables.ContainsKey(name)) {
                return block.Variables[name].Value;
            } else {
                throw new VariableNotFoundException($"A variable with the name {name} was not found.");
            }
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
