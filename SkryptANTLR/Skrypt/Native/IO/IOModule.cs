using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Skrypt {
    public class IOModule : BaseModule {
        public override string Name => "IO";

        public IOModule(Engine engine) : base(engine) {
            //CreateProperty("Path", engine.CreateString(engine.FileHandler.File));
        }

        public static BaseObject Read(Engine engine, BaseObject self, Arguments arguments) {
            var destination = arguments.GetAs<StringInstance>(0);

            var str = engine.CreateString(engine.FileHandler.Read(destination));

            return str;
        }

        public static BaseObject Write(Engine engine, BaseObject self, Arguments arguments) {
            var destination = arguments.GetAs<StringInstance>(0);
            var content = arguments.GetAs<BaseObject>(1).ToString();

            engine.FileHandler.Write(destination, content);

            return null;
        }
    }
}
