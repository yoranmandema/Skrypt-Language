﻿using System;
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

        public Engine() {
            expressionInterpreter   = new ExpressionInterpreter(this);
            templateMaker           = new TemplateMaker(this);

            NumberConstructor       = new NumberConstructor(this);
            Number                  = new NumberObject(this);

            StringConstructor       = new StringConstructor(this);
            String                  = new StringObject(this);
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

        public BaseValue SetValue (string name, BaseDelegate value) {
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
    }
}
