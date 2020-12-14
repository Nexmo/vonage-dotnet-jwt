using System;
using Xunit;
using Vonage.JwtGeneration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;

namespace Vonage.JwtGeneration.Test
{
    public class JwtTest
    {

        private string _mockAppId = "ffffffff-ffff-ffff-ffff-ffffffffffff";
        private string _mockPKCS1 = @"-----BEGIN RSA PRIVATE KEY-----
MIICXAIBAAKBgQCRgWt83vGoI2vx+BIu1R39nLDvGLEvC8R4drrIvsiJkAvIlVZt
PlbeoifYJGDQwtlAR3a8i+B3/AP5tZEoWw+z+VWLX50aRjzHyTn22ih8OeGDoiBw
N3ysCTfQ/x8sDER6uSn8ElxfB9AEZTcRA+4rCRbmj+YLV/Nm+qSNoOIM4wIDAQAB
AoGAC8IZnY2mmZ/DKVqSnZY7RjNTWP710odw6QsvLOm96t/pE9x9j3ZqLrOL5LuL
11Lnm3oq7jGfghKrf5JcmJZDPnhWoGgZvtqFizt1l6y1GY/xlooWhOzEuK9kIrBS
PDhOjnvmLrQIB88Rjgq0LkxjNYsCa5d1zslkB2SfM7sOF4ECQQDkEzk/J0KnAa14
+T00BD5apQsDMU7Tz3aPA1IbmqsKY6wOuHcxrFBMmw3crce6jjq32w36samIPSwG
ucf/JngtAkEAo1IjyJLlVlFA74lGTJTbZ5drrotoYm/YeAnbe6Rhj6pXDfz+lVdQ
5ta/B0TKa1UEgx7pHs39Tmpl2IdfC5ozTwJBANVvL/lrsjI7na1CAQZ2miuVm9Kn
CA+rbFW1U9dFTJ7yW4eDFPhFOvgVeklzzx9EDqsTsedS70XxiQvaO9EInRkCQFKn
TELCzNvNTU6sq24wW4VmpXF1TgObVPMTEgfV3iYF7/69Td4ojWH1xkGYd9Sv9xOg
vhv/5bUctaRKhjhp9pMCQE8BLxzAMlS81dobP3GrCRLdlN/y9R7pu2hyURFFXUw5
j0hq3fgBZz1QLpLxY3TfkM3oFDVhpGvskzjINLk6hxc=
-----END RSA PRIVATE KEY-----";
        private string _mockPKCS8 = @"-----BEGIN PRIVATE KEY-----
MIIEvQIBADANBgkqhkiG9w0BAQEFAASCBKcwggSjAgEAAoIBAQCT3O/qC06UUu54
SGM3hZy6c0ipXIxCxjqS3RGF5q7+UFMwjjCzXHrTfZ6K7vxiEYZo56/OjXpd19K3
7SwsFrlgHUpXBRez2F/dsgy8pvt0uazL5eUqGYE79uqN6RhX0JiMpoY6qsflNPee
ZBJOx7s++YnyQvS/mYZMHtsfGvY8liK3lT3fNtzZtGsF0uY3JlBgkxqQ7vdl6ohL
3HKTbf7niRV8dqQZGyDxOfkBEdif66MkPcyLfQ3F2s8CtKrvGdwPDitjW9cxlH8k
tv9gxACDFtAWnry8izCpgQNNISHDI/SZ8Rf80G0ql3BsPzF66cX5BSVpwmNP0kSE
+4Dggfw7AgMBAAECggEAKH+U/oeGSD3Grw80jZp86Nx2hFyi1g8xL9R43jHmsCUU
A/KOCDJGOfLoH6mBWuLt64G5t1ssrtNUFahSNukqcNbU66yrZ0jWSQRhVLJvoPLS
Dy6ya6t8qA3jBGdZkYPCpJNfpGXuRisRv0IteYJfGMqEK+SG4IuOKv8wiP57fvA9
kcesFszv94ALLB/TjEkr4wRE0AiU65W+XFvgsgeI8br7FanNCF4V1610r3hfRbjk
a3X5JsUszXQWTnxYv5tG8SLxjTAjF7dsDE96wDXpZ41e6ZZb1k661tC+83ykuxlz
D9waP1yu7xeYSFD693bkKfl9CXVRnNn5UT9BS4h2oQKBgQDDuCtgKdeeaIpXyoxl
TBZDSbb+3Y4G3cUkVjFvhcwHvtVmmdegDLsTqEWapnE0WDRGBVz4bfFdi1Z5XTEO
ckYvsI6EaBW3Bo1koDfNXNxT2hHhm2fIVJOoEXvnXLmx7a2IbP4xyd3CHh0Cjx9Z
AMOCqmTuqQPR1VB0EmbPqEsf8wKBgQDBZ3NAkJcjrjvADdn5xXycGrVRhUO3vSIy
U7UJh4oiYgHMQWSISB3J1HICm9FK4rYp6mOZ1LhVg6rfm34z5FbCo+X8Q/TqLMKm
bhWAAN88p8gcykOgn2FeW1PWewjvT1ArhbKpkLhydgIgDWztQ7py302Jr66jBKc0
1WDxTRGMmQKBgAfCN0X6oqeO8V0FlIc3evJz66My2TyAch48pH0NSsdL013b32Zi
2s+urgOxcW9nx7q237ahdR4GNgldnmI6OXoOf7fUAHhe9B/3Ef88HSfdzzOoW3bf
k3LoLoc/b8UT7PsphvImVHorg27kiZOXqih15MZpQNOCp0vSpuy4eTHtAoGAN/YK
CC2OPfnFOi4H21jEVJr5ygvIa1rjkTJdWNOKKaa4JHTrdO+BBwxcrNqPNZ7h3MEA
btt5Nu0xPSBN5Q/19r3b5yF2tWecLvH9cJtP/MoDgikYZlqXnujIGnBhRnVpmh5G
cv/4Ds6MkN+xm/mT8ncghW17F5paE1SGh2uoX0kCgYEAuf5fYLyTw48unXO1hyFu
fBB/+QZpfqIqZxmweJxD0zX35dGO0F2zs1H0Ob7fRP0z4dqrXzyROCzNpPq5VJG8
w6LOfaPU795axQLDVfa0Hxp+p1aqn04HbatSePbDsWv2tP21ZJZRLcxnYMlaJPSu
KiElKOVAmMLlRxie4xWNR10=
-----END PRIVATE KEY-----";


        [Fact]
        public void TestFullAcls()
        {
            var expected = @"{""paths"":{""/*/users/**"":{},""/*/conversations/**"":{},""/*/sessions/**"":{},""/*/devices/**"":{},""/*/image/**"":{},""/*/media/**"":{},""/*/applications/**"":{},""/*/push/**"":{},""/*/knocking/**"":{}}}";
            var acls = Acls.FullAcls();
            var json = JsonConvert.SerializeObject(acls);
            Assert.Equal(expected, json);
        }

        [Fact]
        public void GenerateTokenNoAcls()
        {
            var generator = new JwtGenerator(_mockAppId, _mockPKCS1);
            var jwt = generator.GenerateJwt();
            JToken acls;
            var decoded = JsonConvert.DeserializeObject<JObject>(generator.DecodeJwt(jwt));
            Assert.Equal(_mockAppId, decoded["application_id"].ToString());
            Assert.False(decoded.TryGetValue("acl", out acls));
        }

        [Fact]
        public void GenerateTokenWithFullAcls()
        {
            var generator = new JwtGenerator(_mockAppId, _mockPKCS1, Acls.FullAcls());
            var expected = @"{""paths"":{""/*/users/**"":{},""/*/conversations/**"":{},""/*/sessions/**"":{},""/*/devices/**"":{},""/*/image/**"":{},""/*/media/**"":{},""/*/applications/**"":{},""/*/push/**"":{},""/*/knocking/**"":{}}}";
            var jwt = generator.GenerateJwt();            
            var decoded = JsonConvert.DeserializeObject<JObject>(generator.DecodeJwt(jwt));
            Assert.Equal(_mockAppId, decoded["application_id"].ToString());
            Assert.Equal(expected, Regex.Replace(decoded["acls"].ToString(),@"\s+",""));
        }
        
        [Fact]
        public void TestSingleAcl()
        {
            var expected = @"{""paths"":{""/*/users/**"":{}}}";
            var acls = new Acls 
            { 
                Paths = new List<AclPath> 
                { 
                    new AclPath 
                    { 
                        ApiVersion = "*", ResourceType = "users", Resource = "**", AccessLevels = new object() 
                    } 
                } 
            };
            var generator = new JwtGenerator(_mockAppId, _mockPKCS1, acls);
            var jwt = generator.GenerateJwt();
            var decoded = JsonConvert.DeserializeObject<JObject>(generator.DecodeJwt(jwt));
            Assert.Equal(_mockAppId, decoded["application_id"].ToString());
            Assert.Equal(expected, Regex.Replace(decoded["acls"].ToString(), @"\s+", ""));
        }

        [Fact]
        public void TestPkcs8KeyGeneration()
        {
            var generator = new JwtGenerator(_mockAppId, _mockPKCS8);
            var jwt = generator.GenerateJwt();            
            var decoded = JsonConvert.DeserializeObject<JObject>(generator.DecodeJwt(jwt));
            Assert.Equal(_mockAppId, decoded["application_id"].ToString());
        }
        
        [Fact]
        public void TestFromFile()
        {
            var generator = JwtGeneratorFactory.CreateGeneratorWithFilePath(_mockAppId, "./mock.key");
            var jwt = generator.GenerateJwt();
            var decoded = JsonConvert.DeserializeObject<JObject>(generator.DecodeJwt(jwt));
            Assert.Equal(_mockAppId, decoded["application_id"].ToString());
        }

        [Fact]
        public void TestFactorFromKey()
        {
            var generator = JwtGeneratorFactory.CreateGenerator(_mockAppId, _mockPKCS8);
            var jwt = generator.GenerateJwt();
            var decoded = JsonConvert.DeserializeObject<JObject>(generator.DecodeJwt(jwt));
            Assert.Equal(_mockAppId, decoded["application_id"].ToString());
        }

        [Fact]
        public void TestBadKey()
        {
            try
            {
                var generator = new JwtGenerator(_mockAppId, "blahblah");
            }
            catch(ArgumentException ex)
            {
                Assert.Equal("Invalid Private Key provided", ex.Message);
            }            
        }

        [Fact]
        public void TestBadPath()
        {
            try
            {
                JwtGeneratorFactory.CreateGeneratorWithFilePath(_mockAppId, "./not/a/valid/path.key");
            }
            catch (FileNotFoundException ex)
            {
                Assert.Equal("No ./not/a/valid/path.key file found", ex.Message);
            }
        }
    }
}
