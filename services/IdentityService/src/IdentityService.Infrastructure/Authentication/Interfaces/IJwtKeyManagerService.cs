using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using IdentityService.Domain.Models;

namespace IdentityService.Infrastructure.Authentication.Interfaces;

/// <summary>
/// Interface for managing JWT keys and tokens.
/// </summary>
/// <remarks>
/// This interface/service should only be used within the Infrastructure project.
/// </remarks>
public interface IJwtKeyManagerService
{
    /// <summary>
    /// Gets the ID of the active key.
    /// </summary>
    string ActiveKeyId { get; }

    /// <summary>
    /// Gets the active private key.
    /// </summary>
    RSA GetActivePrivateKey();

    /// <summary>
    /// Gets the active public key.
    /// </summary>
    RSA GetActivePublicKey();

    /// <summary>
    /// Gets the active security key.
    /// </summary>
    RsaSecurityKey GetActiveSecurityKey();

    /// <summary>
    /// Gets all public keys.
    /// </summary>
    IReadOnlyDictionary<string, RSA> GetAllPublicKeys();

    /// <summary>
    /// Gets the JSON Web Key information for all keys.
    /// </summary>
    IEnumerable<JsonWebKeyInfo> GetJsonWebKeys();

    /// <summary>
    /// Gets a public key by its ID.
    /// </summary>
    RSA GetPublicKey(string keyId);
}
