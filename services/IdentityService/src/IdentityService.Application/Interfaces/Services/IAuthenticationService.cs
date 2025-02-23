using IdentityService.Application.Models;
using FluentResults;
namespace IdentityService.Application.Interfaces.Services;

public interface IAuthenticationService
{
    Task<Result<AuthResultDto>> RegisterUserAsync(CreateUserDto user, string password);
    Task<Result<AuthResultDto>> AuthenticateAsync(LoginUserDto user);
    Task<Result<AuthResultDto>> RefreshTokenAsync(string refreshToken);
}
