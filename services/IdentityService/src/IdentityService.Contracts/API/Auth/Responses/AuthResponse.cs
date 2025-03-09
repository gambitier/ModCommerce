namespace IdentityService.Contracts.API.Auth.Responses;

/// <summary>
/// OAuth 2.0 Authorization Response
/// https://datatracker.ietf.org/doc/html/rfc6749#section-4.1.4
/// https://datatracker.ietf.org/doc/html/rfc6749#section-5.1
/// </summary>
public record AuthResponse(
    string AccessToken,
    string TokenType,
    int ExpiresIn,
    string? RefreshToken,

    /// <summary>
    /// Access Token Scope
    /// https://datatracker.ietf.org/doc/html/rfc6749#section-3.3
    /// </summary>
    string? Scope
);
