namespace IdentityService.Application.Services;

public interface ITokenService
{
    string GenerateJwtToken(string userId, string email);
}
