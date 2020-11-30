using System.IO;
using System.Threading.Tasks;

namespace Vonage.Jwt
{
    public class JwtGeneratorFactory
    {
        /// <summary>
        /// Creates a JwtGenerator from an application Id and Private key Path
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="privateKeyPath"></param>
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
        /// <param name="applicationId"></param>
        /// <param name="privateKeyPath"></param>
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
        /// <param name="applicationId"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public static JwtGenerator CreateGenerator(string applicationId, string privateKey, Acls acls = null)
        {
            return new JwtGenerator(applicationId, privateKey, acls);
        }
    }
}
