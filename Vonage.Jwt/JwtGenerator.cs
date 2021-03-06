﻿using Jose;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;

namespace Vonage.JwtGeneration
{
    /// <summary>
    /// This class allows for the flexible generation of JWT tokens.
    /// You can construct it with an Application Id, a choice of a raw private key or path to a private key
    /// and optionally you can add a set of <see cref="Acls"/> to specify what access what accesses the JWT has
    /// </summary>
    public class JwtGenerator : IDisposable
    {
        /// <summary>
        /// Your Application's private key
        /// </summary>
        public string PrivateKey { get; private set; }

        /// <summary>
        /// The Application Id from your <see href="https://dashboard.nexmo.com/applications">Vonage API Account</see>
        /// </summary>
        public string ApplicationId { get; private set; }

        /// <summary>
        /// Time to live of a token generated by this JWT Generator in seconds, defaults to 15 min (900 seconds)
        /// </summary>
        public uint TokenTimeToLive { get; set; } = 900;

        /// <summary>
        /// The access controls created when this generator creates a token
        /// </summary>
        public Acls Acls { get; set; }

        /// <summary>
        /// The decomposed parameters of the provided RSA key
        /// </summary>
        private RSAParameters _parameters;


        private RSACryptoServiceProvider _rsa = new RSACryptoServiceProvider();

        /// <summary>
        /// Construct a JWT generator from an application id and private key
        /// </summary>
        /// <param name="applicationId">The application Id from your 
        /// <see href="https://dashboard.nexmo.com/applications">Vonage API Account</see></param>
        /// <param name="privateKey">The Private Key for your Vonage Application, can either be a raw string or a file path to a key</param>
        /// <param name="acls">The <see cref="Acls">Acls</see> for the token, these indicate a resource version, name, and record
        /// and indicate the JWT bearer's level of access to different API endpoints. use <see cref="Acls.FullAcls"/> to generate a full set of ACLs,
        /// no ACLs will be added by default.</param>
        /// <exception cref="ArgumentException">Throws an Argument exception if provided key is not valid</exception>
        public JwtGenerator(string applicationId, string privateKey, Acls acls=null)
        {
            if (privateKey.IndexOfAny(Path.GetInvalidFileNameChars()) > 0 && File.Exists(privateKey))
            {
                using (var reader = File.OpenText(privateKey))
                {
                    PrivateKey = reader.ReadToEnd();
                }
            }
            else
            {
                PrivateKey = privateKey;
            }
            ApplicationId = applicationId;
            Acls = acls;
            SetupRsaParameters();
        }        

        /// <summary>
        /// extra step at the end of construction to full initalize the RSA parameters.
        /// </summary>
        private void SetupRsaParameters()
        {
            using (var sr = new StringReader(PrivateKey))
            {
                var pemReader = new PemReader(sr);
                var kp = pemReader.ReadObject();
                RsaPrivateCrtKeyParameters privateRsaParams;
                if (kp == null)
                {
                    throw new ArgumentException($"Invalid Private Key provided");
                }
                if (kp is AsymmetricCipherKeyPair) 
                {
                    privateRsaParams = (kp as AsymmetricCipherKeyPair).Private as RsaPrivateCrtKeyParameters;
                }
                else
                {
                    privateRsaParams = kp as RsaPrivateCrtKeyParameters;
                }                
                _parameters = DotNetUtilities.ToRSAParameters(privateRsaParams);
                _rsa.ImportParameters(_parameters);
            }
        }


        /// <summary>
        /// Generates a JWT based off of the Application Id And Private Key provided.
        /// </summary>
        /// <returns></returns>
        public string GenerateJwt()
        {
            var currentTime = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var claims = new
            {
                application_id = ApplicationId,
                iat = currentTime.TotalSeconds,
                exp = currentTime.TotalSeconds + TokenTimeToLive,
                jti = Guid.NewGuid(),
                acls = Acls
            };
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            var json = JsonConvert.SerializeObject(claims, settings);
            return JWT.Encode(json, _rsa, algorithm: JwsAlgorithm.RS256);
        }

        /// <summary>
        /// Decodes a JWT generated by this generator. Can be used for debugging
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public string DecodeJwt(string token)
        {
            return JWT.Decode(token, _rsa, alg: JwsAlgorithm.RS256);
        }

        /// <summary>
        /// Disposes underlying system resources used by generator.
        /// </summary>
        public void Dispose()
        {
            _rsa.Dispose();
        }
    }
}
