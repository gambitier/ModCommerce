using FluentResults;
using IdentityService.Domain.Models;

namespace IdentityService.Domain.Interfaces.AuthenticationServices;

public interface ITokenService
{
    Task<Result<AuthToken>> GenerateToken(string userId, string email);
    Task<Result<AuthToken>> RefreshToken(string refreshToken);
    JsonWebKey[] GetJsonWebKeys();
}
