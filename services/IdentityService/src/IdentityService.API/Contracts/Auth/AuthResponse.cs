namespace IdentityService.API.Contracts.Auth;

/// <summary>
/// OAuth 2.0 Authorization Response
/// https://datatracker.ietf.org/doc/html/rfc6749#section-4.1.4
/// https://datatracker.ietf.org/doc/html/rfc6749#section-5.1
/// </summary>
public class AuthResponse
{
    public required string AccessToken { get; set; }
    public required string TokenType { get; set; } = "Bearer";
    public int ExpiresIn { get; set; }
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Access Token Scope
    /// https://datatracker.ietf.org/doc/html/rfc6749#section-3.3
    /// </summary>
    public string? Scope { get; set; }
}
