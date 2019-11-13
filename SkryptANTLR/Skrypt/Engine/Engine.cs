using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4;
using Skrypt.ANTLR;
using Skrypt.Runtime;
using System.Diagnostics;
using System.Reflection;

namespace Skrypt {
    public partial class Engine {
        public BaseObject CompletionValue => Visitor.LastResult;
        public Stack<Call> CallStack { get; internal set; } = new Stack<Call>();

        internal Stopwatch SW { get; private set; }
        internal SkryptParser Parser { get; private set; }
        internal SkryptVisitor Visitor { get; private set; }
        internal ExpressionInterpreter ExpressionInterpreter { get; private set; }
        internal TemplateMaker TemplateMaker { get; private set; }
        internal SkryptParser.ProgramContext ProgramContext { get; private set; }
        internal List<BaseTrait> StandardTraits { get; private set; } = new List<BaseTrait>();

        #region Traits
        internal EnumerableTrait Enumerable { get; private set; }
        internal IteratorTrait Iterator { get; private set; }
        internal AddableTrait AddableTrait { get; private set; }
        internal SubtractableTrait SubtractableTrait { get; private set; }
        internal DividableTrait DividableTrait { get; private set; }
        internal MultiplicableTrait MultiplicableTrait { get; private set; }

        #endregion

        #region Types
        internal NumberType Number { get; private set; }
        internal StringType String { get; private set; }
        internal BooleanType Boolean { get; private set; }
        internal VectorType Vector { get; private set; }
        internal VectorType Vector2 { get; private set; }
        internal VectorType Vector3 { get; private set; }
        internal VectorType Vector4 { get; private set; }
        internal ArrayType Array { get; private set; }
        #endregion

        #region Modules
        internal DebugModule Debug { get; private set; }
        internal MathModule Math { get; private set; }
        internal IOModule IO { get; private set; }
        internal ReflectionModule Reflection { get; private set; }
        #endregion


        //public Dictionary<string, Variable> Globals { get; set; } = new Dictionary<string, Variable>();
        public ErrorHandler ErrorHandler { get; set; }
        public IFileHandler FileHandler { get; set; }

        public LexicalEnvironment GlobalEnvironment { get; set; }
        //public LexicalEnvironment CurrentEnvironment { get; set; }

        public Engine() {
            GlobalEnvironment       = new LexicalEnvironment();
            //CurrentEnvironment      = GlobalEnvironment;

            ErrorHandler            = new ErrorHandler(this);
            ExpressionInterpreter   = new ExpressionInterpreter(this);
            TemplateMaker           = new TemplateMaker(this);
            FileHandler             = new DefaultFileHandler(this);
            Visitor                 = new SkryptVisitor(this);

            Enumerable              = FastAdd(new EnumerableTrait(this));
            Iterator                = FastAdd(new IteratorTrait(this));
            AddableTrait            = FastAdd(new AddableTrait(this));
            SubtractableTrait       = FastAdd(new SubtractableTrait(this));
            DividableTrait          = FastAdd(new DividableTrait(this));
            MultiplicableTrait      = FastAdd(new MultiplicableTrait(this));

            Number                  = FastAdd(new NumberType(this));
            String                  = FastAdd(new StringType(this));
            Boolean                 = FastAdd(new BooleanType(this));
            Vector                  = FastAdd(new VectorType(this));
            Vector2                 = FastAdd(new Vector2Type(this));
            Vector3                 = FastAdd(new Vector3Type(this));
            Vector4                 = FastAdd(new Vector4Type(this));
            Array                   = FastAdd(new ArrayType(this));

            Math                    = FastAdd(new MathModule(this));
            IO                      = FastAdd(new IOModule(this));
            Reflection              = FastAdd(new ReflectionModule(this));
            Debug                   = FastAdd(new DebugModule(this));

            SW = Stopwatch.StartNew();
        }

        public Engine DoFile(string file) {
            FileHandler.File = file;
            FileHandler.Folder = System.IO.Path.GetDirectoryName(file);
            FileHandler.BaseFolder = System.IO.Path.GetDirectoryName(file);

            var code = FileHandler.Read(file);

            return Run(code);
        }
        public Engine DoRelativeFile (string file) {
            var oldFile = FileHandler.File;
            var newFile = System.IO.Path.Combine(FileHandler.Folder, file);

            FileHandler.File = newFile;
            FileHandler.Folder = System.IO.Path.GetDirectoryName(newFile);

            var code = FileHandler.Read(file);
            var result = Run(code);

            FileHandler.File = oldFile;
            FileHandler.Folder = System.IO.Path.GetDirectoryName(oldFile);

            return result;
        }

        public Engine Run(string code) {
            var errorListener = new ErrorListener(this);

            var inputStream = new AntlrInputStream(code);
            var skryptLexer = new SkryptLexer(inputStream) {
                Engine = this
            };

            skryptLexer.RemoveErrorListeners();
            skryptLexer.AddErrorListener(errorListener);

            var commonTokenStream = new CommonTokenStream(skryptLexer);

            Parser = new SkryptParser(commonTokenStream) {
                Engine = this,
                TokenStream = commonTokenStream
                //Engine = this,
                //ErrorHandler = new BailErrorStrategy()
            };

            Parser.RemoveErrorListeners();
            Parser.AddErrorListener(errorListener);

            Parser.GlobalEnvironment = GlobalEnvironment;

            ProgramContext = Parser.program();
            Parser.LinkLexicalEnvironments(ProgramContext, GlobalEnvironment);

            if (!ErrorHandler.HasErrors) {
                Visitor.CurrentEnvironment = GlobalEnvironment;
                Visitor.Visit(ProgramContext);
            }

            return this;
        }

        public Engine ReportErrors () {
            ErrorHandler.ReportAllErrors();

            return this;
        }

        public T FastAdd<T> (T obj) where T : BaseObject {
            SetGlobal(obj.Name, obj);

            return obj;
        }

        public Engine CreateGlobals () {
            var block = ProgramContext.block();

            foreach (var v in block.Variables) {
                GlobalEnvironment.AddVariable(v.Value);
            }

            return this;
        }

        internal void SetGlobal (string name, BaseObject value) {
            if (GlobalEnvironment.Variables.ContainsKey(name)) {
                GlobalEnvironment.Variables[name].Value = value;
            } else {
                GlobalEnvironment.Variables[name] = new Variable(name, value);
            }
        }

        public void Print(string message) {
            Console.WriteLine(message);
        }

        public BaseObject SetValue(string name, BaseObject value) {
            SetGlobal(name, value);

            return value;
        }

        public BaseObject SetValue(string name, bool value) {
            var val = CreateBoolean(value);

            SetGlobal(name, val);

            return val;
        }

        public BaseObject SetValue (string name, MethodDelegate value) {
            var val = new FunctionInstance(this, value);

            SetGlobal(name,val);

            return val;
        }

        public BaseObject GetValue(string name) {
            var block = ProgramContext.block();

            if (block.Variables.ContainsKey(name)) {
                return block.Variables[name].Value;
            } else if (GlobalEnvironment.Variables.ContainsKey(name)) {
                return GlobalEnvironment.Variables[name].Value;
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

        public ArrayInstance CreateArray(BaseObject[] values) {
            return (ArrayInstance)Array.Construct(values);
        }
    }
}
