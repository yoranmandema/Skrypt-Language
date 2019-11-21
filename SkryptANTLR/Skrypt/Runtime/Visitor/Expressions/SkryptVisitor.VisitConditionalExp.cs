﻿using System;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Skrypt.ANTLR;

namespace Skrypt {
    public partial class SkryptVisitor : SkryptBaseVisitor<BaseObject> {
        public override BaseObject VisitConditionalExp(SkryptParser.ConditionalExpContext context) {
            var condition = Visit(context.expression(0));
            var value = DefaultResult;

            if (condition.IsTrue()) {
                value = Visit(context.expression(1));
            } else {
                value = Visit(context.expression(2));
            }

            if (value is IValue noref) value = noref.Copy();

            LastResult = value;

            return value;
        }
    }
}
