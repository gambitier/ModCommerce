using IdentityService.Domain.Constants;

namespace IdentityService.Domain.Models;

public record AuthTokenInfo(
    string AccessToken,
    TokenType TokenType,
    int ExpiresIn,
    string? RefreshToken = null,
    string? Scope = null
);
