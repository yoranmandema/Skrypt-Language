using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt.Extensions.JSON {
    public class JSONConverter : JsonConverter {
        public override bool CanConvert(Type objectType) {

            //Console.WriteLine(objectType);
            //Console.WriteLine(typeof(SkryptObject).IsAssignableFrom(objectType));

            return typeof(SkryptObject).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            var skryptObject = value as SkryptObject;

            writer.WriteStartObject();

            foreach (var property in skryptObject.Members) {
                if (property.Value.value is FunctionInstance functionInstance) {
                    writer.WritePropertyName(property.Key);
                    serializer.Serialize(writer, $"{property.Key}()");
                } else {
                    writer.WritePropertyName(property.Key);
                    serializer.Serialize(writer, property.Value.value.ToString());
                }

                //writer.WriteValue(serializer.);
            }

            writer.WriteEndObject();
        }
    }
}
