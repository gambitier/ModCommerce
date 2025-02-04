namespace IdentityService.Domain.Interfaces.AuthenticationServices;

public interface ITokenService
{
    string GenerateJwtToken(string userId, string email);
}
