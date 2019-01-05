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
            var args = new double[arguments.Length];

            for (var i = 0; i < arguments.Length; i++) {
                var arg = arguments[i];

                if (arg is NumberInstance) {
                    args[i] = (NumberInstance)arg;
                }
                else {
                    throw new Exception("Vector components can only be made from numbers!");
                }
            }

            return Construct(args);
        }
    }
}
