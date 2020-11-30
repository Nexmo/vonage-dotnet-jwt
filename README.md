# Vonage Dotnet JWT

<img src="https://developer.nexmo.com/assets/images/Vonage_Nexmo.svg" height="48px" alt="Nexmo is now known as Vonage" />

## Welcome to Vonage

If you're new to Vonage, you can [sign up for a Vonage API account](https://dashboard.nexmo.com/sign-up?utm_source=DEV_REL&utm_medium=github&utm_campaign=vonage-dotnet-jwt) and get some free credit to get you started.

## Overview

This library is designed to facilitate the generation of JWTs both for legacy Vonage API calls as well as more advanced use cases, such as conversations. For full docs on how JWTs are used in the Vonage APIs see our [documentation website](https://developer.nexmo.com/concepts/guides/authentication#json-web-tokens-jwt). To see slightly more advanced use cases with the Voange Conversations api see [this article](https://developer.nexmo.com/conversation/guides/jwt-acl)

## Generating JWTs

### Create a Token with a Raw Private Key

To generate a JWT, or any number of JWTs given a set of parameters you must first initialize a `JwtGenerator`. You can do this by passing in your Vonage Application id along with your application's private key into the `JwtGenerator` constructor, Subsequently you can then generate a JWT using the generator's `GenerateJwt` method:

```csharp
var generator = new Vonage.JwtGeneration.JwtGenerator(applicationId, privateKeyPath);
var jwt = generator.GenerateJwt();
```

### Change Expiration Time of Token

Each token generated by the `JwtGenerator` will carry an `exp` claim set to 15 minutes after the time the token was generated. To change the 15 minutes timer, simply change the `JwtGenerator` `TokenTimeToLive` do the desired value in seconds (default is 900).

### Generate a Token from a Private Key path

The JwtGenerator's constructor will accept either a raw private key or a file path, however if you are using a file path it is recommended that you use the `JwtGeneratorFactory.CreateGeneratorWithFilePathAsync` method to read the file asynchronously.

```csharp
var generator = await JwtGeneratorFactory.CreateGeneratorWithFilePathAsync(applicationId, privateKeyPath);
var jwt = generator.GenerateJwt();
```

### Adding Acls to your tokens

If you are using the Vonage Conversations API or the client SDK, you will need to add particular Acls to your tokens. The easiest way to do this is to simply add Full permissions to each Token. This is the method shown [here](https://developer.nexmo.com/conversation/guides/jwt-acl#acls). This can be accomplished by making use of the third, optional argument in the `JwtGenerator` constructor, as well as the `Acls.FullAcls` method. 

```csharp
var generator = new JwtGenerator(applicationId, privateKey, Acls.FullAcls());
var jwt = generator.GenerateJwt();
```

### Specifying Acls in Your Tokens

If you do not wish to grant full permissions in your token, you can add permissions to whatever path you desire by adding each path individually to the `Acls` `Paths` list. This will take a set of `AclPath` objects, each of which contains an `ApiVersion`, `ResourceType`, `Resource`, and `AccessLevels`. A fully open Path has those values set to `*`, `resourceType e.g. users`, `**`, `{}` and the path will be formatted as `/ApiVersion/ResourceType/Resource":AccessLevels`.

```csharp
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
var generator = new JwtGenerator(_mockAppId, _mockRsaPrivateKey, acls);
var jwt = generator.GenerateJwt();
```

Will produce the path: `"*/users/**":{}` in the `acls` claim of your JWT

## Getting Help

We love to hear from you so if you have questions, comments or find a bug in the project, let us know! You can either:

* Open an issue on this repository
* Tweet at us! We're [@VonageDev on Twitter](https://twitter.com/VonageDev)
* Or [join the Vonage Developer Community Slack](https://developer.nexmo.com/community/slack)

## Further Reading

* Check out the Developer Documentation at <https://developer.nexmo.com>

<!-- add links to the api reference, other documentation, related blog posts, whatever someone who has read this far might find interesting :) -->

