using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vonage.JwtGeneration
{
    /// <summary>
    /// Specially formated ACLs that a given token is to be given access to
    /// <see href="https://developer.nexmo.com/conversation/guides/jwt-acl#acls">see here</see>
    /// for a more detailed explenation
    /// </summary>
    [JsonConverter(typeof(PathSerializer))]
    public class Acls
    {
        /// <summary>
        /// the set of paths to use for serialization. NOTE: this list will seralize as an object
        /// rather than a standard JSON array
        /// </summary>
        [JsonProperty("paths")]
        public List<AclPath> Paths { get; set; }

        /// <summary>
        /// This generates an ACLS object permitting the bearer to have access to all 
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
