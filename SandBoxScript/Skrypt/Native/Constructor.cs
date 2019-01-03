using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public abstract class Constructor<T> where T : BaseInstance {
        protected readonly Engine _engine;
        protected readonly Template _template;

        public Constructor (Engine engine) {
            _engine = engine;

            _template = _engine.templateMaker.CreateTemplate(typeof(T));
        }

        public abstract T Construct(BaseValue[] arguments);
    }
}
