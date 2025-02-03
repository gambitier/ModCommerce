using IdentityService.Application.Models;

namespace IdentityService.Application.Interfaces.Services;

public interface IAuthenticationService
{
    Task<AuthResultDto> RegisterUserAsync(UserDto user, string password);
    Task<AuthResultDto> AuthenticateAsync(string email, string password);
}
