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
using System.IO;
using Skrypt.CLR;
using Skrypt.Compiling;

namespace Skrypt {
    public partial class SkryptEngine {

        private static readonly ParserOptions DefaultParserOptions = new ParserOptions {
            Tolerant = false,
            ReportErrors = true
        };

        public SkryptObject CompletionValue => Visitor.LastResult;
        public Stack<Call> CallStack { get; internal set; } = new Stack<Call>();

        public readonly long MemoryLimit;

        internal Stopwatch SW { get; private set; }
        internal SkryptParser Parser { get; private set; }
        internal SkryptVisitor Visitor { get; private set; }
        internal ExpressionInterpreter ExpressionInterpreter { get; private set; }
        internal SkryptParser.ProgramContext ProgramContext { get; private set; }
        internal List<BaseTrait> StandardTraits { get; private set; } = new List<BaseTrait>();
        internal TextWriter TextWriter { get; private set; }

        internal static Func<long> GetAllocatedBytesForCurrentThread { get; private set; }
        internal long InitialMemoryUsage { get; private set; }
        internal bool HaltMemory { get; private set; }
        internal long OPS { get; set; }

        internal Dictionary<Type, Func<SkryptEngine, object, SkryptObject>> ImportTypeMappers = new Dictionary<Type, Func<SkryptEngine, object, SkryptObject>> {
            { typeof(bool), (engine, v) => engine.CreateBoolean((bool)v) },
            { typeof(double), (engine, v) => engine.CreateNumber((double)v) },
            { typeof(float), (engine, v) => engine.CreateNumber((float)v) },
            { typeof(decimal), (engine, v) => engine.CreateNumber((double)(decimal)v) },
            { typeof(Int16), (engine, v) => engine.CreateNumber((Int16)v) },
            { typeof(Int32), (engine, v) => engine.CreateNumber((Int32)v) },
            { typeof(Int64), (engine, v) => engine.CreateNumber((Int64)v) },
            { typeof(UInt16), (engine, v) => engine.CreateNumber((UInt16)v) },
            { typeof(UInt32), (engine, v) => engine.CreateNumber((UInt32)v) },
            { typeof(UInt64), (engine, v) => engine.CreateNumber((UInt64)v) },
            { typeof(byte), (engine, v) => engine.CreateNumber((byte)v) },
            { typeof(string), (engine, v) => engine.CreateString((string)v) }
        };

        internal Dictionary<Type, Func<SkryptObject, object>> ExportTypeMappers = new Dictionary<Type, Func<SkryptObject, object>> {
            { typeof(BooleanInstance), (v) => ((BooleanInstance)v).Value },
            { typeof(NumberInstance), (v) => ((NumberInstance)v).Value },
            { typeof(StringInstance), (v) => ((StringInstance)v).Value }
        };

        private readonly bool _discardGlobal;
        private readonly int _maxRecursionDepth = -1;

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
        internal MemoryModule Memory { get; private set; }
        #endregion


        public ErrorHandler ErrorHandler { get; set; }
        public IFileHandler FileHandler { get; set; }
        public TemplateMaker TemplateMaker { get; private set; }
        public LexicalEnvironment GlobalEnvironment { get; set; }

        static SkryptEngine() {
            var methodInfo = typeof(GC).GetMethod("GetAllocatedBytesForCurrentThread");

            if (methodInfo != null) {
                GetAllocatedBytesForCurrentThread = (Func<long>)Delegate.CreateDelegate(typeof(Func<long>), null, methodInfo);
            }
        }

        public SkryptEngine() : this(null) { }

        public SkryptEngine(Options options) {
            GlobalEnvironment = new LexicalEnvironment();

            ErrorHandler = new DefaultErrorHandler(this);
            ExpressionInterpreter = new ExpressionInterpreter(this);
            TemplateMaker = new TemplateMaker(this);
            FileHandler = new DefaultFileHandler(this);
            Visitor = new SkryptVisitor(this);

            Enumerable = FastAdd(new EnumerableTrait(this));
            Iterator = FastAdd(new IteratorTrait(this));
            AddableTrait = FastAdd(new AddableTrait(this));
            SubtractableTrait = FastAdd(new SubtractableTrait(this));
            DividableTrait = FastAdd(new DividableTrait(this));
            MultiplicableTrait = FastAdd(new MultiplicableTrait(this));

            Number = FastAdd(new NumberType(this));
            String = FastAdd(new StringType(this));
            Boolean = FastAdd(new BooleanType(this));
            Vector = FastAdd(new VectorType(this));
            Vector2 = FastAdd(new Vector2Type(this));
            Vector3 = FastAdd(new Vector3Type(this));
            Vector4 = FastAdd(new Vector4Type(this));
            Array = FastAdd(new ArrayType(this));

            Math = FastAdd(new MathModule(this));
            IO = FastAdd(new IOModule(this));
            Memory = FastAdd(new MemoryModule(this));
            Debug = FastAdd(new DebugModule(this));

            if (options != null) {
                _discardGlobal = options.DiscardGlobal;
                _maxRecursionDepth = options.MaxRecursionDepth;
                MemoryLimit = options.MaxMemory;
                HaltMemory = options.MemoryHalt;
            }
        }

        public SkryptEngine DoFile(string file) {
            SetFile(file);
            FileHandler.BaseFolder = System.IO.Path.GetDirectoryName(file);

            var code = FileHandler.Read(file);

            return Execute(code);
        }

        private void SetFile(string file) {
            FileHandler.File = file;
            FileHandler.Folder = Path.GetDirectoryName(file);
            ErrorHandler.File = file;
        }

        public SkryptEngine DoRelativeFile(string file) {
            var oldFile = FileHandler.File;
            var newFile = System.IO.Path.Combine(FileHandler.Folder, file);

            SetFile(newFile);

            var code = FileHandler.Read(file);
            var result = Execute(code);

            SetFile(oldFile);

            return result;
        }


        public void ResetMemoryUsage() {
            if (GetAllocatedBytesForCurrentThread != null) {
                InitialMemoryUsage = GetAllocatedBytesForCurrentThread();
            }
        }

        public SkryptParser.ProgramContext ParseProgram(string source, ParserOptions options) {
            ErrorHandler.Source = source;

            var errorHandler = new CompileErrorHandler(this, source) {
                Tolerant = options.Tolerant
            };

            var errorListener = new ErrorListener(this, errorHandler);

            var inputStream = new AntlrInputStream(source);
            var skryptLexer = new SkryptLexer(inputStream) {
                Engine = this,
                ErrorHandler = errorHandler
            };

            skryptLexer.RemoveErrorListeners();
            skryptLexer.AddErrorListener(errorListener);

            var commonTokenStream = new CommonTokenStream(skryptLexer);

            Parser = new SkryptParser(commonTokenStream) {
                Engine = this,
                TokenStream = commonTokenStream,
                ErrorHandler = errorHandler
            };

            Parser.RemoveErrorListeners();
            Parser.AddErrorListener(errorListener);

            Parser.GlobalEnvironment = GlobalEnvironment;

            var program = Parser.program();

            Parser.LinkLexicalEnvironments(program, GlobalEnvironment);

            if (errorHandler.Errors.Any() && options.ReportErrors) {
                var sorted = errorHandler.Errors.OrderBy(x => x.File).ThenBy(x => x.Line).ThenBy(x => x.Column);

                foreach (var err in sorted) {
                    ErrorHandler.ReportError(err);
                }
            }

            return program;
        }

        public SkryptEngine Execute(SkryptParser.ProgramContext program) {
            if (SW == null) {
                SW = Stopwatch.StartNew();
            } else {
                SW.Start();
            }

            ProgramContext = program;

            if (MemoryLimit > 0) {
                ResetMemoryUsage();
            }

            Visitor.CurrentEnvironment = GlobalEnvironment;
            Visitor.Visit(ProgramContext);

            if (!_discardGlobal) CreateGlobals();

            SW.Stop();

            return this;
        }

        public SkryptEngine Execute(string code, ParserOptions parserOptions) {
            var program = ParseProgram(code, parserOptions);

            return Execute(program);
        }

        public SkryptEngine Execute(string code) {
            return Execute(code, DefaultParserOptions);
        }

        public T FastAdd<T>(T obj) where T : SkryptObject {
            SetGlobal(obj.Name, obj);

            return obj;
        }

        internal SkryptEngine CreateGlobals() {
            var block = ProgramContext.block();

            foreach (var v in block.LexicalEnvironment.Variables) {
                GlobalEnvironment.AddVariable(v.Value);
            }

            return this;
        }

        internal void SetGlobal(string name, SkryptObject value) {
            if (GlobalEnvironment.Variables.ContainsKey(name)) {
                GlobalEnvironment.Variables[name].Value = value;
            } else {
                GlobalEnvironment.Variables[name] = new Variable(name, value);
            }
        }

        public SkryptEngine SetValue(string name, SkryptObject value) {
            SetGlobal(name, value);

            return this;
        }

        public SkryptEngine SetValue(string name, bool value) {
            var val = CreateBoolean(value);

            SetGlobal(name, val);

            return this;
        }

        public SkryptEngine SetValue(string name, double value) {
            var val = CreateNumber(value);

            SetGlobal(name, val);

            return this;
        }

        public SkryptEngine SetValue(string name, string value) {
            var val = CreateString(value);

            SetGlobal(name, val);

            return this;
        }

        public SkryptEngine SetValue(string name, MethodDelegate value) {
            var val = new FunctionInstance(this, value);

            SetGlobal(name, val);

            return this;
        }

        public SkryptEngine SetValue(string name, Delegate value) {
            var val = new FunctionInstance(this, new CLRFunction(this, value));

            SetGlobal(name, val);

            return this;
        }

        public SkryptObject GetValue(string name) {
            var block = ProgramContext?.block();

            if (block != null && block.LexicalEnvironment.Variables.ContainsKey(name)) {
                return block.LexicalEnvironment.Variables[name].Value;
            } else if (GlobalEnvironment.Variables.ContainsKey(name)) {
                return GlobalEnvironment.Variables[name].Value;
            } else {
                throw new VariableNotFoundException($"A variable with the name {name} was not found.");
            }
        }

        public bool TryGetValue(string name, out SkryptObject value) {
            if (GlobalEnvironment.Variables.ContainsKey(name)) {
                value = GlobalEnvironment.Variables[name].Value;

                return true;
            }

            value = null;
            return false;
        }

        public SkryptEngine AddImportTypeMapper(Type type, Func<SkryptEngine, object, SkryptObject> func) {
            if (ImportTypeMappers.ContainsKey(type)) {
                throw new SkryptException($"Typemapper for type {type.FullName} already exists!");
            }

            ImportTypeMappers.Add(type, func);

            return this;
        }

        public SkryptEngine AddExportTypeMapper(Type type, Func<SkryptObject, object> func) {
            if (ExportTypeMappers.ContainsKey(type)) {
                throw new SkryptException($"Typemapper for type {type.FullName} already exists!");
            }

            ExportTypeMappers.Add(type, func);

            return this;
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

        public ArrayInstance CreateArray(SkryptObject[] values) {
            return (ArrayInstance)Array.Construct(values);
        }
    }
}
