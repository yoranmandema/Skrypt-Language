using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4;
using Skrypt.ANTLR;
using Skrypt.Runtime;

namespace Skrypt {
    public class Engine {
        public BaseValue CompletionValue => Visitor.LastResult;

        internal SkryptParser Parser;
        internal SkryptVisitor Visitor;

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

        internal SkryptParser.ProgramContext ProgramContext;
        internal Dictionary<string, Variable> Globals = new Dictionary<string, Variable>();

        public ErrorHandler ErrorHandler = new ErrorHandler(); 

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

            Visitor                 = new SkryptVisitor(this);
        }

        public Engine Run(string code) {
            var inputStream = new AntlrInputStream(code);
            var skryptLexer = new SkryptLexer(inputStream) {
                Engine = this
            };

            skryptLexer.AddErrorListener(new LexingErrorListener());

            var commonTokenStream = new CommonTokenStream(skryptLexer);

            Parser = new SkryptParser(commonTokenStream) {
                Engine = this,
                ErrorHandler = new BailErrorStrategy()
            };

            Parser.AddErrorListener(new ParsingErrorListener());

            Parser.Globals = Globals;

            ProgramContext = Parser.program();

            if (ErrorHandler.HasErrors) {
                ErrorHandler.ReportAllErrors();

                ErrorHandler.Errors.Clear();
            } else {
                Visitor.Visit(ProgramContext);
            }

            return this;
        }

        public Engine CreateGlobals () {
            var block = ProgramContext.block();

            foreach (var v in block.Variables) {
                Globals[v.Key] = v.Value;
            }

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
            } else if (Globals.ContainsKey(name)) {
                return Globals[name].Value;
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
