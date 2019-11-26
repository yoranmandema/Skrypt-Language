using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System.Collections.Generic;
using Skrypt.ANTLR;
using System.Linq;
using Skrypt.Runtime;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<SkryptObject> {
        public SkryptObject LastResult { get; private set; }
        
        public LexicalEnvironment CurrentEnvironment { get; internal set; }
        private readonly SkryptEngine _engine;
        private SkryptObject accessed;

        public SkryptVisitor (SkryptEngine engine) {
            _engine = engine;
        }

        public override SkryptObject VisitImportStatement(SkryptParser.ImportStatementContext context) => DefaultResult;
        public override SkryptObject VisitFunctionStatement(SkryptParser.FunctionStatementContext context) => DefaultResult;
        public override SkryptObject VisitModuleStatement(SkryptParser.ModuleStatementContext context) => null;
        public override SkryptObject VisitStructStatement(SkryptParser.StructStatementContext context) => null;
        public override SkryptObject VisitTraitStatement(SkryptParser.TraitStatementContext context) => null;
        public override SkryptObject VisitTraitImplStatement(SkryptParser.TraitImplStatementContext context) => null;
    }
}