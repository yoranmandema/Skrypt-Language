using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt {
    public class ExceptionInstance : SkryptInstance {
        public override string Name => "Exception";

        public ExceptionInstance(SkryptEngine engine, string message, string stackTrace) : base(engine) {
            CreateProperty("message", engine.CreateString(message), false, true);
            CreateProperty("stackTrace", engine.CreateString(stackTrace), false, true);
        }
    }
}
