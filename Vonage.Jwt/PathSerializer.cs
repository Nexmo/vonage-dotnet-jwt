using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vonage.JwtGeneration
{
    public class PathSerializer : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Acls);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            var acls = (Acls)value;
            writer.WritePropertyName("paths");            
            writer.WriteStartObject();
            foreach (var path in acls.Paths)
            {
                
                writer.WritePropertyName($"/{path.ApiVersion}/{path.ResourceType}/{path.Resource}");
                serializer.Serialize(writer, path.AccessLevels);                
            }
            writer.WriteEndObject();
            writer.WriteEndObject();            
        }
    }
}
