using Microsoft.IdentityModel.Tokens;
using IdentityService.Infrastructure.Authentication.Options;
using System.Security.Cryptography;
using IdentityService.Domain.Models;
using IdentityService.Infrastructure.Authentication.Interfaces;
using Microsoft.Extensions.Options;
namespace IdentityService.Infrastructure.Authentication.Services;

public sealed class JwtKeyManagerService : IJwtKeyManagerService
{
    private readonly Dictionary<string, RSA> _publicKeys;
    private readonly RSA _activePrivateKey;
    private readonly string _activeKeyId;

    public JwtKeyManagerService(IOptions<JwtOptions> opts)
    {
        var options = opts.Value;
        _publicKeys = new Dictionary<string, RSA>();

        var activeKey = options.Keys.Single(k => k.IsActive);
        _activeKeyId = activeKey.KeyId;

        // Load all public keys
        foreach (var keyConfig in options.Keys)
        {
            var rsa = RSA.Create();
            rsa.ImportFromPem(keyConfig.PublicKeyPem);
            _publicKeys[keyConfig.KeyId] = rsa;
        }

        // Load active private key
        _activePrivateKey = RSA.Create();
        _activePrivateKey.ImportFromPem(activeKey.PrivateKeyPem);
    }

    public string ActiveKeyId => _activeKeyId;

    public RSA GetActivePrivateKey() => _activePrivateKey;

    public RSA GetPublicKey(string keyId) => _publicKeys[keyId];

    public RSA GetActivePublicKey() => _publicKeys[_activeKeyId];

    public IReadOnlyDictionary<string, RSA> GetAllPublicKeys() => _publicKeys;

    public RsaSecurityKey GetActiveSecurityKey()
    {
        return new RsaSecurityKey(GetActivePrivateKey())
        {
            KeyId = _activeKeyId
        };
    }

    public IEnumerable<Domain.Models.JsonWebKey> GetJsonWebKeys()
    {
        return _publicKeys.Select(kvp =>
        {
            var rsa = kvp.Value;
            var jwk = new Microsoft.IdentityModel.Tokens.JsonWebKey
            {
                Kty = JsonWebAlgorithmsKeyTypes.RSA,
                Kid = kvp.Key,
                Use = "sig",
                Alg = SecurityAlgorithms.RsaSha256,
                N = Base64UrlEncoder.Encode(rsa.ExportParameters(false).Modulus),
                E = Base64UrlEncoder.Encode(rsa.ExportParameters(false).Exponent)
            };

            return new Domain.Models.JsonWebKey(
                Kty: jwk.Kty,
                Kid: jwk.Kid,
                Use: jwk.Use!,
                Alg: jwk.Alg,
                N: jwk.N,
                E: jwk.E
            );
        });
    }
}
