using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandBoxScript {
    class VectorConstructor : Constructor<VectorInstance> {
        public VectorConstructor(Engine engine) : base(engine) { }

        public VectorInstance Construct(params double[] args) {
            var obj = default(VectorInstance);

            switch (args.Length) {
                case 3:
                    obj = new Vector3Instance(_engine, args[0], args[1], args[2]);
                    break;
            }
           
            obj.GetProperties(_template);



            return obj;
        }

        public override VectorInstance Construct(BaseValue[] arguments) {
            var args = new double[arguments.Length];

            for (var i = 0; i < arguments.Length; i++) {
                var arg = arguments[i];

                if (arg is NumberInstance) {
                    args[i] = (NumberInstance)arg;
                } else {
                    throw new Exception("Vector components can only be made from numbers!");
                }
            }

            return Construct(args);
        }
    }
}
