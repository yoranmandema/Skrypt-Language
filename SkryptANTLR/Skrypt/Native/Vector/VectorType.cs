using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class VectorType : BaseType {
        private readonly Template _template;

        public VectorType(Engine engine) : base(engine) {
            _template = engine.templateMaker.CreateTemplate(typeof(NumberInstance));
        }

        public BaseInstance Construct(params double[] args) {
            var obj = default(VectorInstance);

            switch (args.Length) {
                case 2:
                    obj = new Vector2Instance(Engine, args[0], args[1]);
                    break;
                case 3:
                    obj = new Vector3Instance(Engine, args[0], args[1], args[2]);
                    break;
                case 4:
                    obj = new Vector4Instance(Engine, args[0], args[1], args[2], args[3]);
                    break;
            }

            obj.GetProperties(_template);

            return obj;
        }

        public override BaseInstance Construct(Arguments arguments) {
            var length = Math.Max(arguments.Length, 2);
            var args = new double[length];

            for (var i = 0; i < length; i++) {
                args[i] = arguments.GetAs<NumberInstance>(i);
            }

            return Construct(args);
        }
    }
}
