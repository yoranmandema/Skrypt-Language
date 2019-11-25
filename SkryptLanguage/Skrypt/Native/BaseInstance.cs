using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class BaseInstance : BaseObject {
        public BaseInstance(Engine engine) : base(engine) { }
        public BaseType TypeObject { get; set; }

        public static BaseObject Type(Engine engine, BaseObject self) {
            return (self as BaseInstance).TypeObject;
        }

        public bool HasTrait<T> () where T : BaseTrait {
            return TypeObject.Traits.OfType<T>().Any();
        }
    }
}
