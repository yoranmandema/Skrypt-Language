using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;
using Skrypt.ANTLR;
using System.Linq;
using Skrypt.Runtime;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<BaseObject> {
        public BaseObject LastResult { get; private set; }
        
        public LexicalEnvironment CurrentEnvironment { get; internal set; }
        private readonly Engine _engine;
        private BaseObject accessed;

        public SkryptVisitor (Engine engine) {
            _engine = engine;
        }

        public override BaseObject VisitImportStatement(SkryptParser.ImportStatementContext context) => DefaultResult;
        public override BaseObject VisitFunctionStatement(SkryptParser.FunctionStatementContext context) => DefaultResult;
        public override BaseObject VisitModuleStatement(SkryptParser.ModuleStatementContext context) => null;
        public override BaseObject VisitStructStatement(SkryptParser.StructStatementContext context) => null;
        public override BaseObject VisitTraitStatement(SkryptParser.TraitStatementContext context) => null;
        public override BaseObject VisitTraitImplStatement(SkryptParser.TraitImplStatementContext context) => null;
    }
}