using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vonage.JwtGeneration
{
    /// <summary>
    /// Custom JSON serializer to specially serialize AclPaths
    /// </summary>
    public class PathSerializer : JsonConverter
    {

        /// <summary>
        /// Checks to ensure the object passed in is of the type Acls
        /// </summary>
        /// <param name="objectType"></param>        
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Acls);
        }

        /// <summary>
        /// DO NOT USE THIS CLASS FOR READING JSON OBJECTS
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes the Acls in a special format for the Vonage API.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
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
