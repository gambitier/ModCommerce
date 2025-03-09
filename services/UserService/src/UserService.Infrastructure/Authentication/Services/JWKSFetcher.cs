using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using UserService.Infrastructure.Authentication.Options;
using Microsoft.IdentityModel.Tokens;

namespace UserService.Infrastructure.Authentication.Services;

public interface IJwksFetcher
{
    // TODO: use fluent errors package and return errors instead of throwing exceptions
    Task<List<(string KeyId, RSA RSAKey)>> FetchJWKSKeys();
}

public class JsonWebKey
{
    [JsonPropertyName("kty")]
    public required string Kty { get; init; }

    [JsonPropertyName("kid")]
    public required string Kid { get; init; }

    [JsonPropertyName("use")]
    public required string Use { get; init; }

    [JsonPropertyName("alg")]
    public required string Alg { get; init; }

    [JsonPropertyName("n")]
    public required string N { get; init; }

    [JsonPropertyName("e")]
    public required string E { get; init; }

    /// <summary>
    /// Converts the JSON Web Key to an RSA key.
    /// </summary>
    /// <returns>The RSA key.</returns>
    public RSA ToRSA()
    {
        var rsa = RSA.Create();
        rsa.ImportParameters(new RSAParameters
        {
            Modulus = Base64UrlEncoder.DecodeBytes(N),
            Exponent = Base64UrlEncoder.DecodeBytes(E)
        });
        return rsa;
    }
}

public class JwksResponse
{
    [JsonPropertyName("keys")]
    public required List<JsonWebKey> Keys { get; init; }
}

/// <summary>
/// Fetches the JWKS keys from the JWKS URI.
/// Note
/// - it this class should only be responsible for fetching the JWKS keys
/// - it should not be responsible for caching the JWKS keys or anything else
/// </summary>
public class JwksFetcher : IJwksFetcher
{
    private readonly HttpClient _httpClient;
    private readonly JwtOptions _jwtOptions;
    private string _jwksUri = null!;

    public JwksFetcher(HttpClient httpClient, IOptions<JwtOptions> jwtOptions)
    {
        _httpClient = httpClient;
        _jwtOptions = jwtOptions.Value;
    }

    private string GetJwksUri()
    {
        // TODO: use this when identity service has a discovery endpoint
        // if (!string.IsNullOrEmpty(_jwksUri))
        // {
        //     return _jwksUri;
        // }
        // var discoveryUrl = $"{_jwtSettings.Authority}/.well-known/openid-configuration";
        // var response = await _httpClient.GetStringAsync(discoveryUrl);
        // var discoveryDoc = JsonSerializer.Deserialize<DiscoveryDocument>(response);
        // if (string.IsNullOrEmpty(discoveryDoc?.JwksUri))
        // {
        //     // TODO: should return error result instead of throwing exception
        //     throw new Exception("JWKS URI not found in OIDC discovery document.");
        // }
        // _jwksUri = discoveryDoc.JwksUri;
        // return _jwksUri;

        _jwksUri = $"{_jwtOptions.Authority}/{_jwtOptions.JWKSUrlPath}";
        return _jwksUri;
    }

    public async Task<List<(string KeyId, RSA RSAKey)>> FetchJWKSKeys()
    {
        var jwksUrl = GetJwksUri();
        var response = await _httpClient.GetStringAsync(jwksUrl);
        var jwks = JsonSerializer.Deserialize<JwksResponse>(response);

        if (jwks == null)
        {
            // TODO: should return error result instead of throwing exception
            throw new InvalidOperationException("Failed to deserialize JWKS response.");
        }

        return jwks.Keys
            .Where(k => !string.IsNullOrEmpty(k.Kid))
            .Select(k => (k.Kid, k.ToRSA()))
            .ToList();
    }
}
