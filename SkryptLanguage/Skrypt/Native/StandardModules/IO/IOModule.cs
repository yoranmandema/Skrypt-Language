using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Skrypt {
    public class IOModule : SkryptModule {
        public override string Name => "IO";

        public IOModule(SkryptEngine engine) : base(engine) {
            //CreateProperty("Path", engine.CreateString(engine.FileHandler.File));
        }

        public static SkryptObject Read(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            var destination = arguments.GetAs<StringInstance>(0);

            var str = engine.CreateString(engine.FileHandler.Read(destination));

            return str;
        }

        public static SkryptObject Write(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            var destination = arguments.GetAs<StringInstance>(0);
            var content = arguments.GetAs<SkryptObject>(1).ToString();

            engine.FileHandler.Write(destination, content);

            return null;
        }

        public static SkryptObject ReadAsync(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            var destination = arguments.GetAs<StringInstance>(0);
            var function = arguments.GetAs<FunctionInstance>(1);

            engine.FileHandler.ReadAsync(destination, function);

            return null;
        }

        public static SkryptObject WriteAsync(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            var destination = arguments.GetAs<StringInstance>(0);
            var content = arguments.GetAs<SkryptObject>(1).ToString();
            var function = arguments.GetAs<FunctionInstance>(2);

            engine.FileHandler.WriteAsync(destination, content, function);

            return null;
        }
    }
}
