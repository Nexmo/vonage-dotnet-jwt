using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vonage.JwtGeneration
{
    /// <summary>
    /// An object representing a single ACL path,
    /// format of path will be:
    /// <code>"/ApiVersion/ResourceType/Resource":AccessLevels</code>
    /// A standard way to generate one of these is using wild cards, e.g.
    /// <code>"/*/push/**": {}</code>
    /// </summary>
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
        /// the resource being granted access to. typically an id. For all resources use **
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// The access levels you want to provide for the JWT path, defaults to an emptty object (which is all)
        /// </summary>
        public object AccessLevels { get; set; } = new object();
    }
}
