using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class VectorType : BaseType {

        Template Template2;
        Template Template3;
        Template Template4;

        public VectorType(Engine engine) : base(engine) {
            Template = engine.templateMaker.CreateTemplate(typeof(VectorInstance));
            Template2 = engine.templateMaker.CreateTemplate(typeof(Vector2Instance));
            Template3 = engine.templateMaker.CreateTemplate(typeof(Vector3Instance));
            Template4 = engine.templateMaker.CreateTemplate(typeof(Vector4Instance));
        }

        public BaseInstance Construct(params double[] args) {
            var obj = default(VectorInstance);

            switch (args.Length) {
                case 2:
                    obj = new Vector2Instance(Engine, args[0], args[1]);

                    obj.GetProperties(Template2);

                    break;
                case 3:
                    obj = new Vector3Instance(Engine, args[0], args[1], args[2]);

                    obj.GetProperties(Template3);

                    break;
                case 4:
                    obj = new Vector4Instance(Engine, args[0], args[1], args[2], args[3]);

                    obj.GetProperties(Template4);

                    break;
            }

            obj.GetProperties(Template);
            obj.TypeObject = this;

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
