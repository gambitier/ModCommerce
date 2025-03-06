using FluentResults;
using IdentityService.Domain.Models;

namespace IdentityService.Domain.Interfaces.AuthenticationServices;

public interface ITokenService
{
    Task<Result<AuthTokenInfo>> GenerateToken(string userId, string email);
    Task<Result<AuthTokenInfo>> RefreshToken(string refreshToken);
    JsonWebKeyInfo GetJsonWebKey();
}
