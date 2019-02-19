using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt{
    public class ArrayType : BaseType {
        public ArrayType(Engine engine) : base(engine) {
            Template = engine.templateMaker.CreateTemplate(typeof(ArrayInstance));

            var enumerable = ImplementTrait(engine.Enumerable);

            enumerable["Get"].Value = new FunctionInstance(engine, (e, s, args) => {
                var array = s as ArrayInstance;
                var index = (int)args.GetAs<NumberInstance>(0);

                return index < array.SequenceValues.Count ? array.SequenceValues[index] : null;
            });

            enumerable["IsInRange"].Value = new FunctionInstance(engine, (e, s, args) => {
                var array = s as ArrayInstance;

                return Engine.CreateBoolean(args.GetAs<NumberInstance>(0) < array.SequenceValues.Count);
            });

            enumerable["GetIterator"].Value = new FunctionInstance(engine, (e, s, args) => {
                return Engine.Iterator.Construct(new BaseObject[] { s as ArrayInstance });
            });
        }

        public BaseInstance Construct(BaseObject[] values) {
            var obj = new ArrayInstance(Engine);

            for (int i = 0; i < values.Length; i++) {
                obj.SequenceValues.Add(values[i]);
            }

            obj.GetProperties(Template);
            obj.TypeObject = this;

            return obj;
        }

        public override BaseInstance Construct(Arguments arguments) {
            return Construct(arguments.Values);
        }
    }
}
