using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4;

namespace Skrypt.ANTLR {
    public partial class SkryptParser {
        public SkryptEngine Engine { get; internal set; }
        public LexicalEnvironment GlobalEnvironment;
        public ITokenStream TokenStream { get; internal set; }
        public CompileErrorHandler CompileErrorHandler { get; internal set; }

        public partial class ModuleStmntContext : IScopedContext {
            public RuleContext Context => this;

            public LexicalEnvironment LexicalEnvironment { get; set; } = new LexicalEnvironment();
        }

        public partial class StructStmntContext : IScopedContext {
            public RuleContext Context => this;
            public LexicalEnvironment LexicalEnvironment { get; set; } = new LexicalEnvironment();
        }

        public partial class TraitStmntContext : IScopedContext {
            public RuleContext Context => this;
            public LexicalEnvironment LexicalEnvironment { get; set; } = new LexicalEnvironment();
        }

        public partial class TraitImplStmntContext : IScopedContext {
            public RuleContext Context => this;
            public LexicalEnvironment LexicalEnvironment { get; set; } = new LexicalEnvironment();
        }

        public partial class BlockContext : IScopedContext {
            public RuleContext Context => this;
            public LexicalEnvironment LexicalEnvironment { get; set; } = new LexicalEnvironment();
        }

        public partial class CatchStmtContext : IScopedContext {
            public RuleContext Context => this;
            public LexicalEnvironment LexicalEnvironment { get; set; } = new LexicalEnvironment();
            public Exception error;
        }

        public partial class FunctionStatementContext : IFunctionContext {
            public RuleContext Context => this;
            public LexicalEnvironment LexicalEnvironment { get; set; } = new LexicalEnvironment();
            public StmntBlockContext StmntBlock => stmntBlock();
            public SkryptObject ReturnValue { get; set; }
            public JumpState JumpState { get; set; }

            public FunctionStatementContext Clone() {
                return (FunctionStatementContext)MemberwiseClone();
            }
        }

        public partial class FnLiteralContext : IFunctionContext {
            public RuleContext Context => this;
            public LexicalEnvironment LexicalEnvironment { get; set; } = new LexicalEnvironment();
            public StmntBlockContext StmntBlock => stmntBlock();
            public SkryptObject ReturnValue { get; set; }
            public JumpState JumpState { get; set; }
        }

        public partial class WhileStatementContext : ILoopContext {
            public JumpState JumpState { get; set; } = JumpState.None;
        }

        public partial class ContinueStmntContext {
            internal ILoopContext Statement;
            public Skrypt.JumpState JumpState = Skrypt.JumpState.None;
        }

        public partial class BreakStmntContext {
            internal ILoopContext Statement;
            public Skrypt.JumpState JumpState = Skrypt.JumpState.None;
        }

        public partial class ForStatementContext : ILoopContext {
            public JumpState JumpState { get; set; } = JumpState.None;
        }

        public partial class FnLiteralContext {
            internal FunctionInstance value;
        }

        public partial class ReturnStmntContext {
            internal IFunctionContext Statement;
        }

        public partial class FunctionCallExpContext {
            public string CallFile { get; set; }
        }


        private void CreateProperty(SkryptObject target, IScopedContext ctx, ParserRuleContext propertyTree, bool isPrivate) {
            IToken nameToken = null;

            if (propertyTree.GetChild(0) is MemberDefinitionStatementContext assignCtx) {
                nameToken = assignCtx.name().NAME().Symbol;
            }
            else if (propertyTree.GetChild(0) is FunctionStatementContext fnCtx) {
                nameToken = fnCtx.name().NAME().Symbol;
            }
            else if (propertyTree.GetChild(0) is ModuleStatementContext moduleCtx) {
                nameToken = moduleCtx.name().NAME().Symbol;
            }
            else if (propertyTree.GetChild(0) is StructStatementContext structCtx) {
                nameToken = structCtx.name().NAME().Symbol;
            }

            var value = ctx.LexicalEnvironment.Variables[nameToken.Text].Value;

            if (value == null) {
                CompileErrorHandler.TolerateError(nameToken, "Field can't be set to an undefined value.");
            }

            target.CreateProperty(nameToken.Text, value, isPrivate, ctx.LexicalEnvironment.Variables[nameToken.Text].IsConstant);
        }

        private bool ContextIsIn(RuleContext context, Type[] types) {
            foreach (var t in types) {
                if (context.parent.parent.GetType() == t) {
                    return true;
                }
            }

            return false;
        }

        private IToken GetPropertyNameToken(ParserRuleContext propertyTree) {
            if (propertyTree.GetChild(0) is AssignNameStatementContext assignCtx) {
                return assignCtx.name().NAME().Symbol;
            }
            else if (propertyTree.GetChild(0) is FunctionStatementContext fnCtx) {
                return fnCtx.name().NAME().Symbol;
            }
            else if (propertyTree.GetChild(0) is MemberDefinitionStatementContext mdCtx) {
                return mdCtx.name().NAME().Symbol;
            }

            return null;
        }

        private IScopedContext GetDefinitionBlock(string name, RuleContext ctx) {
            IScopedContext scope = null;
            IScopedContext first = null;

            RuleContext currentContext = ctx;
            while (currentContext.Parent != null) {
                if (currentContext is IScopedContext scopedCtx) {
                    if (first == null) first = scopedCtx;

                    if (scopedCtx.LexicalEnvironment.Variables.ContainsKey(name)) {
                        scope = scopedCtx;
                        break;
                    }
                }

                currentContext = currentContext.Parent;
            }

            if (scope == null) {
                scope = first;
            }

            return scope;
        }

        internal IScopedContext GetDefinitionBlock(RuleContext ctx) {
            while (ctx.Parent != null) {
                if (ctx is IScopedContext scopedCtx) {
                    return scopedCtx;
                }

                ctx = ctx.Parent;
            }

            return null;
        }

        //private T GetFirstOfType<T>(RuleContext ctx) where T : RuleContext {
        //    RuleContext currentContext = ctx;

        //    while (currentContext.Parent != null) {
        //        if (currentContext is T typeCtx) {
        //            return typeCtx;
        //        }

        //        currentContext = currentContext.Parent;
        //    }

        //    return null;
        //}

        private IFunctionContext GetFirstFunctionStatement(RuleContext ctx) {
            RuleContext currentContext = ctx;

            while (currentContext.Parent != null) {
                if (currentContext is IFunctionContext typeCtx) {
                    return typeCtx;
                }

                currentContext = currentContext.Parent;
            }

            return null;
        }

        private ILoopContext GetFirstLoopStatement(RuleContext ctx) {
            RuleContext currentContext = ctx;

            while (currentContext.Parent != null) {
                if (currentContext is ILoopContext loopCtx) {
                    return loopCtx;
                }

                currentContext = currentContext.Parent;
            }

            return null;
        }

        private Variable GetReference(string name, IScopedContext scope) {
            if (scope.LexicalEnvironment.Variables.ContainsKey(name)) {
                return scope.LexicalEnvironment.GetVariable(name);
            }
            else if (this.GlobalEnvironment.Variables.ContainsKey(name)) {
                return this.GlobalEnvironment.Variables[name];
            }

            return null;
        }

        public void LinkLexicalEnvironments(RuleContext context, LexicalEnvironment parentEnvironment) {
            if (context is IScopedContext scoped) {
                parentEnvironment.AddChild(scoped.LexicalEnvironment);
                parentEnvironment = scoped.LexicalEnvironment;
                parentEnvironment.Context = context;
            }

            for (int i = 0; i < context.ChildCount; i++) {
                var child = context.GetChild(i);

                if (child is RuleContext ruleContext) {
                    LinkLexicalEnvironments(ruleContext, parentEnvironment);
                }
            }
        }
    }
}
