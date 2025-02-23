using IdentityService.Domain.Models;

namespace IdentityService.Domain.Interfaces.AuthenticationServices;

public interface ITokenService
{
    AuthTokenInfo GenerateToken(string userId, string email);
}
