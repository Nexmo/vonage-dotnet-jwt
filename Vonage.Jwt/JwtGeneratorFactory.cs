using System.IO;
using System.Threading.Tasks;

namespace Vonage.JwtGeneration
{
    /// <summary>
    /// Provides utility methods for creating a <see cref="JwtGenerator"/>
    /// </summary>
    public class JwtGeneratorFactory
    {
        /// <summary>
        /// Creates a JwtGenerator from an application Id and Private key Path asynchrnously
        /// </summary>
        /// <param name="applicationId">The application Id from your 
        /// <see href="https://dashboard.nexmo.com/applications">Vonage API Account</see></param>
        /// <param name="privateKeyPath">a path to the private key for your Vonage Application, can either be a raw string or a file path to a key</param>
        /// <param name="acls">The <see cref="Acls">Acls</see> for the token, these indicate a resource version, name, and record
        /// and indicate the JWT bearer's level of access to different API endpoints. use <see cref="Acls.FullAcls"/> to generate a full set of ACLs,
        /// no ACLs will be added by default.</param>
        /// <exception cref="System.IO.FileNotFoundException">Thrown if File Doesn't exist</exception>
        /// <returns></returns>
        public static async Task<JwtGenerator> CreateGeneratorWithFilePathAsync(string applicationId, string privateKeyPath, Acls acls = null)
        {
            if (!File.Exists(privateKeyPath))
            {
                throw new FileNotFoundException($"No {privateKeyPath} file found");
            }

            using (var reader = File.OpenText(privateKeyPath))
            {
                var privateKey = await reader.ReadToEndAsync();
                return new JwtGenerator(applicationId, privateKey, acls);
            }
        }

        /// <summary>
        /// Creates a JwtGenerator from an application Id and Private key Path
        /// </summary>
        /// <param name="applicationId">The application Id from your 
        /// <see href="https://dashboard.nexmo.com/applications">Vonage API Account</see></param>
        /// <param name="privateKeyPath">A path to the private key for your Vonage Application, can either be a raw string or a file path to a key</param>
        /// <param name="acls">The <see cref="Acls">Acls</see> for the token, these indicate a resource version, name, and record
        /// and indicate the JWT bearer's level of access to different API endpoints. use <see cref="Acls.FullAcls"/> to generate a full set of ACLs,
        /// no ACLs will be added by default.</param>
        /// <returns></returns>
        public static JwtGenerator CreateGeneratorWithFilePath(string applicationId, string privateKeyPath, Acls acls = null)
        {
            return CreateGeneratorWithFilePathAsync(applicationId, privateKeyPath, acls).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Creates a JwtGenerator from an Application Id and raw private key. 
        /// If using a File path it's advised to use the CreateGeneratorWithFilePathAsync
        /// so system resources are accessed asynchrnously.
        /// </summary>
        /// <param name="applicationId">The application Id from your 
        /// <see href="https://dashboard.nexmo.com/applications">Vonage API Account</see></param>
        /// <param name="privateKey">The Private Key for your Vonage Application, can either be a raw string or a file path to a key</param>
        /// <param name="acls">The <see cref="Acls">Acls</see> for the token, these indicate a resource version, name, and record
        /// and indicate the JWT bearer's level of access to different API endpoints. use <see cref="Acls.FullAcls"/> to generate a full set of ACLs,
        /// no ACLs will be added by default.</param>
        /// <returns></returns>
        public static JwtGenerator CreateGenerator(string applicationId, string privateKey, Acls acls = null)
        {
            return new JwtGenerator(applicationId, privateKey, acls);
        }
    }
}
