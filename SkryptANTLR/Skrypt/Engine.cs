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

        internal NumberType Number;
        internal StringType String;
        internal BooleanType Boolean;
        internal VectorType Vector;
        internal ArrayType Array;

        internal MathModule Math;

        internal SkryptParser.ProgramContext ProgramContext;
        internal Dictionary<string, Variable> Globals = new Dictionary<string, Variable>();

        public ErrorHandler ErrorHandler = new ErrorHandler();
        public IFileHandler IOHandler = new DefaultIOHandler();

        public Engine() {
            expressionInterpreter   = new ExpressionInterpreter(this);
            templateMaker           = new TemplateMaker(this);

            Number                  = new NumberType(this);
            String                  = new StringType(this);
            Boolean                 = new BooleanType(this);
            Vector                  = new VectorType(this);
            Array                   = new ArrayType(this);

            Math                    = new MathModule(this);

            Visitor                 = new SkryptVisitor(this);
        }

        public Engine DoFile(string file) {
            IOHandler.File = file;
            IOHandler.Directory = System.IO.Path.GetDirectoryName(file);

            var code = IOHandler.Read(file);

            return Run(code);
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
            return (NumberInstance)Number.Construct(value);
        }

        public StringInstance CreateString(string value) {
            return (StringInstance)String.Construct(value);
        }

        public BooleanInstance CreateBoolean(bool value) {
            return (BooleanInstance)Boolean.Construct(value);
        }

        public Vector2Instance CreateVector2(double x, double y) {
            return (Vector2Instance)Vector.Construct(x, y);
        }

        public Vector3Instance CreateVector3(double x, double y, double z) {
            return (Vector3Instance)Vector.Construct(x, y, z);
        }

        public Vector4Instance CreateVector4(double x, double y, double z, double w) {
            return (Vector4Instance)Vector.Construct(x, y, z, w);
        }

        public ArrayInstance CreateArray(BaseValue[] values) {
            return (ArrayInstance)Array.Construct(values);
        }
    }
}
