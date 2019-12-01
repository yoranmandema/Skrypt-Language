using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class ArrayIteratorType : SkryptType {
        public ArrayIteratorType(SkryptEngine engine) : base(engine) {
            Template = Engine.TemplateMaker.CreateTemplate(typeof(ArrayInstance));

            var iteratorTrait = ImplementTrait(engine.Iterator);

            iteratorTrait["Current"].value = new FunctionInstance(Engine, (e, s, args) => {
                var iterator = s as ArrayIteratorInstance;

                return iterator.Index > -1 && iterator.Index < iterator.Array.SequenceValues.Count ? iterator.Array.SequenceValues[iterator.Index] : null;
            });

            iteratorTrait["Next"].value = new FunctionInstance(Engine, (e, s, args) => {
                var iterator = s as ArrayIteratorInstance;

                iterator.Index++;

                return iterator.Index > -1 && iterator.Index < iterator.Array.SequenceValues.Count ? iterator.Array.SequenceValues[iterator.Index] : null;
            });

            iteratorTrait["HasNext"].value = new FunctionInstance(Engine, (e, s, args) => {
                var iterator = s as ArrayIteratorInstance;
                var index = iterator.Index + 1;

                return Engine.CreateBoolean(index > -1 && index < iterator.Array.SequenceValues.Count);
            });

            iteratorTrait["Reset"].value = new FunctionInstance(Engine, (e, s, args) => {
                var iterator = s as ArrayIteratorInstance;

                iterator.Index = -1;

                return null;
            });
        }

        public SkryptInstance Construct(SkryptObject[] values) {
            var obj = new ArrayIteratorInstance(Engine);

            obj.GetProperties(Template);
            obj.TypeObject = this;
            obj.Array = values[0] as ArrayInstance;

            return obj;
        }

        public override SkryptInstance Construct(Arguments arguments) {
            return Construct(arguments.Values);
        }
    }
}
