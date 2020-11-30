using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vonage.Jwt
{
    [JsonConverter(typeof(PathSerializer))]
    public class Acls
    {
        [JsonProperty("paths")]
        public List<AclPath> Paths { get; set; }

        /// <summary>
        /// This 
        /// </summary>
        /// <returns></returns>
        public static Acls FullAcls ()
        {
            return new Acls
            {
                Paths = new List<AclPath>
                {
                    new AclPath{ApiVersion="*", ResourceType="users",Resource="**"},
                    new AclPath{ApiVersion="*", ResourceType="conversations",Resource="**"},
                    new AclPath{ApiVersion="*", ResourceType="sessions",Resource="**"},
                    new AclPath{ApiVersion="*", ResourceType="devices",Resource="**"},
                    new AclPath{ApiVersion="*", ResourceType="image",Resource="**"},
                    new AclPath{ApiVersion="*", ResourceType="media",Resource="**"},
                    new AclPath{ApiVersion="*", ResourceType="applications",Resource="**"},
                    new AclPath{ApiVersion="*", ResourceType="push",Resource="**"},
                    new AclPath{ApiVersion="*", ResourceType="knocking",Resource="**"},
                }
            };
        }
    }
}
