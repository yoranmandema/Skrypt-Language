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

        public static SkryptObject ToJSON(SkryptEngine engine, SkryptObject self, Arguments arguments) {
            var value = arguments[0];
            var indented = arguments.Length > 1 ? arguments.GetAs<BooleanInstance>(1): false;

            var jsonString = JsonConvert.SerializeObject(value, indented ? Formatting.Indented : Formatting.None, new JsonSerializerSettings {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                Converters = new[] { new JSONConverter() }
            });

            return engine.CreateString(jsonString);
        }
    }
}
