using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vonage.JwtGeneration
{
    public class AclPath
    {
        /// <summary>
        /// the name of the type of resource being granted (e.g. users)
        /// </summary>
        public string ResourceType { get; set; }

        /// <summary>
        /// The version of the API being allowed access to (e.g. v0.1), for all versions use *
        /// </summary>
        public string ApiVersion { get; set; }

        /// <summary>
        /// the resource being granted access to. typically an id
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// The access levels you want to provide for the JWT path, defaults to all
        /// </summary>
        public object AccessLevels { get; set; } = new object();
    }
}
