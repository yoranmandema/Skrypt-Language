using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Skrypt.Extensions.JSON {
    public class JSONModule : SkryptModule {
        public JSONModule(SkryptEngine engine) : base(engine) {
        }

        public static SkryptObject ToJson(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            var value = arguments[0];
            var indented = arguments.Length > 1 ? arguments.GetAs<BooleanInstance>(1): false;
            var writeFunctions = arguments.Length > 2 ? arguments.GetAs<BooleanInstance>(2) : false;

            var jsonString = JsonConvert.SerializeObject(value, indented ? Formatting.Indented : Formatting.None, new JsonSerializerSettings {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                Converters = new JsonConverter[] {
                    new SkryptNumberJsonConverter(),
                    new SkryptStringJsonConverter(),
                    new SkryptBooleanJsonConverter(),
                    new SkryptArrayJsonConverter(),
                    new SkryptObjectJsonConverter(writeFunctions)
                }
            });

            return engine.CreateString(jsonString);
        }
    }
}
