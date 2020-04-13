using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skrypt.Extensions.JSON {
    public class SkryptObjectJsonConverter : JsonConverter {

        private bool _writeFunctions;

        public SkryptObjectJsonConverter (bool writeFunctions) {
            _writeFunctions = writeFunctions;
        }

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

                // Serialize Functions
                if (property.Value.value is FunctionInstance functionInstance && _writeFunctions) {
                    writer.WritePropertyName(property.Key);
                    serializer.Serialize(writer, $"{property.Key}()");
                } 
                else {
                    writer.WritePropertyName(property.Key);
                    serializer.Serialize(writer, property.Value.value);
                }

                //writer.WriteValue(serializer.);
            }

            writer.WriteEndObject();
        }
    }
}
