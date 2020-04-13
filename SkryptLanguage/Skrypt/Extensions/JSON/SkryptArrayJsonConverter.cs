using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt.Extensions.JSON {
    class SkryptArrayJsonConverter : JsonConverter {
        public override bool CanConvert(Type objectType) {
            return typeof(ArrayInstance).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            var array = value as ArrayInstance;

            writer.WriteStartArray();

            for (int i = 0; i < array.SequenceValues.Count; i++) {
                var val = array.SequenceValues[i];

                serializer.Serialize(writer, val);
            }

            writer.WriteEndArray();
        }
    }
}
