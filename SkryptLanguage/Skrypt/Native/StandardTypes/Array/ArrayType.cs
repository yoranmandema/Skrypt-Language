using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt{
    public class ArrayType : SkryptType {
        public ArrayType(SkryptEngine engine) : base(engine) {
            Template = Engine.TemplateMaker.CreateTemplate(typeof(ArrayInstance));

            var arrayIteratorType = Engine.FastAdd(new ArrayIteratorType(Engine));
            var enumerable = ImplementTrait(engine.Enumerable);

            enumerable["GetIterator"].value = new FunctionInstance(Engine, (e, s, args) => {
                return arrayIteratorType.Construct(new SkryptObject[] { s as ArrayInstance });
            });
        }

        public SkryptInstance Construct(SkryptObject[] values) {
            var obj = new ArrayInstance(Engine);

            for (int i = 0; i < values.Length; i++) {
                obj.SequenceValues.Add(values[i]);
            }

            obj.GetProperties(Template);
            obj.TypeObject = this;

            return obj;
        }

        public override SkryptInstance Construct(Arguments arguments) {
            return Construct(arguments.Values);
        }
    }
}
