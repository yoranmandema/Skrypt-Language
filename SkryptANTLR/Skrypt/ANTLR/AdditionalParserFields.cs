using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4;

namespace Skrypt.ANTLR {
    public partial class SkryptParser {
        public Engine Engine { get; internal set; }

        public Dictionary<string, Variable> Globals = new Dictionary<string, Variable>();

        public partial class ModuleStmntContext : IScoped {
            public Dictionary<string, Variable> Variables { get; set; } = new Dictionary<string, Variable>();
            public RuleContext Context => this;
        }

        public partial class StructStmntContext : IScoped {
            public Dictionary<string, Variable> Variables { get; set; } = new Dictionary<string, Variable>();
            public RuleContext Context => this;
        }

        public partial class TraitStmntContext : IScoped {
            public Dictionary<string, Variable> Variables { get; set; } = new Dictionary<string, Variable>();
            public RuleContext Context => this;
        }

        public partial class TraitImplStmntContext : IScoped {
            public Dictionary<string, Variable> Variables { get; set; } = new Dictionary<string, Variable>();
            public RuleContext Context => this;
        }

        public partial class BlockContext : IScoped {
            public Dictionary<string, Variable> Variables { get; set; } = new Dictionary<string, Variable>();
            public RuleContext Context => this;
        }

        public partial class FunctionStatementContext : IFunctionContext {
            public Dictionary<string, Variable> Variables { get; set; } = new Dictionary<string, Variable>();
            public RuleContext Context => this;
            public StmntBlockContext StmntBlock => stmntBlock();
            public BaseObject ReturnValue { get; set; }
            public JumpState JumpState { get; set; }
        }

        public partial class FnLiteralContext : IFunctionContext {
            public Dictionary<string, Variable> Variables { get; set; } = new Dictionary<string, Variable>();
            public RuleContext Context => this;
            public StmntBlockContext StmntBlock => stmntBlock();
            public BaseObject ReturnValue { get; set; }
            public JumpState JumpState { get; set; }
        }

        public partial class WhileStatementContext : ILoop {
            public JumpState JumpState { get; set; } = JumpState.None;
        }

        public partial class ForStatementContext : ILoop {
            public JumpState JumpState { get; set; } = JumpState.None;
        }

        public partial class FunctionCallExpContext {
            public string CallFile { get; set; }
        }

        void CreateProperty(BaseObject target, IScoped ctx, ParserRuleContext propertyTree, bool isPrivate) {
            IToken nameToken = null;

            if (propertyTree.GetChild(0) is MemberDefinitionStatementContext assignCtx) {
                nameToken = assignCtx.name().NAME().Symbol;
            } else if (propertyTree.GetChild(0) is FunctionStatementContext fnCtx) {
                nameToken = fnCtx.name().NAME().Symbol;
            } else if (propertyTree.GetChild(0) is ModuleStatementContext moduleCtx) {
                nameToken = moduleCtx.name().NAME().Symbol;
            } else if (propertyTree.GetChild(0) is StructStatementContext structCtx) {
                nameToken = structCtx.name().NAME().Symbol;
            }

            var value = ctx.Variables[nameToken.Text].Value;

            if (value == null) {
                Engine.ErrorHandler.AddError(nameToken, "Field can't be set to an undefined value.");
            }

            target.CreateProperty(nameToken.Text, value, isPrivate);
        }

        bool ContextIsIn(RuleContext context, Type[] types) {

            foreach (var t in types) {
                if (context.parent.parent.GetType() == t) {
                    return true;
                }
            }

            return false;
        }

        IToken GetPropertyNameToken(ParserRuleContext propertyTree) {
            IToken nameToken = null;

            if (propertyTree.GetChild(0) is AssignNameStatementContext assignCtx) {
                nameToken = assignCtx.name().NAME().Symbol;
            }
            else if (propertyTree.GetChild(0) is FunctionStatementContext fnCtx) {
                nameToken = fnCtx.name().NAME().Symbol;
            }

            return nameToken;
        }

        IScoped GetDefinitionBlock (string name, RuleContext ctx) {
            IScoped scope = null;
            IScoped first = null;

            RuleContext currentContext = ctx;
            while (currentContext.Parent != null) {
                if (currentContext is IScoped scopedCtx) {
                    if (first == null) first = scopedCtx;

                    if (scopedCtx.Variables.ContainsKey(name)) {
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

        IScoped GetDefinitionBlock(RuleContext ctx) {
            while (ctx.Parent != null) {
                if (ctx is IScoped scopedCtx) {
                    return scopedCtx;
                }

                ctx = ctx.Parent;
            }

            return null;
        }

        T GetFirstOfType<T>(RuleContext ctx) where T : RuleContext {
            RuleContext currentContext = ctx;

            while (currentContext.Parent != null) {
                if (currentContext is T typeCtx) {
                    return typeCtx;
                }

                currentContext = currentContext.Parent;
            }

            return null;
        }

        IFunctionContext GetFirstFunctionStatement (RuleContext ctx){
            RuleContext currentContext = ctx;

            while (currentContext.Parent != null) {
                if (currentContext is IFunctionContext typeCtx) {
                    return typeCtx;
                }

                currentContext = currentContext.Parent;
            }

            return null;
        }

        Variable GetReference (string name, IScoped scope) {
            Variable variable = null;

            if (scope.Variables.ContainsKey(name)) {
                variable = scope.Variables[name];
            } else if (this.Globals.ContainsKey(name)) {
                variable = this.Globals[name];
            }

            return variable;
        }
    }
}
