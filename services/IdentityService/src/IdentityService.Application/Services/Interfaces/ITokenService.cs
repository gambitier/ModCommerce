namespace IdentityService.Application.Services.Interfaces;

public interface ITokenService
{
    string GenerateJwtToken(string userId, string email);
}
