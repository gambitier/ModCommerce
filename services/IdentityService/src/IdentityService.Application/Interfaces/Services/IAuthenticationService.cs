using IdentityService.Application.Models;
using FluentResults;
namespace IdentityService.Application.Interfaces.Services;

public interface IAuthenticationService
{
    Task<Result<AuthResultDto>> RegisterUserAsync(UserDto user, string password);
    Task<Result<AuthResultDto>> AuthenticateAsync(string email, string password);
}
