using System.Security.Cryptography;
using System.Linq;

namespace UserService.Infrastructure.Authentication.Services;

public interface IJwksManager
{
    Task<RSA?> GetPublicKey(string kid);
    Task RefreshKeys();
}

public class JwksManager : IJwksManager
{
    private readonly IJwksFetcher _jwksFetcher;
    private Dictionary<string, RSA> _keyCache = new();

    public JwksManager(IJwksFetcher jwksFetcher)
    {
        _jwksFetcher = jwksFetcher;
    }

    public async Task<RSA?> GetPublicKey(string kid)
    {
        if (string.IsNullOrEmpty(kid))
        {
            return null;
        }

        // Try to get from cache first
        if (_keyCache.TryGetValue(kid, out var cachedKey))
        {
            return cachedKey;
        }

        // If key not found, refresh keys
        await RefreshKeys();

        // Try again after refresh
        return _keyCache.TryGetValue(kid, out var key) ? key : null;
    }

    public async Task RefreshKeys()
    {
        var keys = await _jwksFetcher.FetchJWKSKeys();
        _keyCache = keys
            .Where(k => !string.IsNullOrEmpty(k.KeyId))
            .ToDictionary(k => k.KeyId, k => k.RSAKey);
    }
}