using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt.Extensions.JSON {
    public class SkryptNumberJsonConverter : JsonConverter {
        public override bool CanConvert(Type objectType) {
            return typeof(NumberInstance).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            writer.WriteValue((value as NumberInstance).Value);
        }
    }
}
