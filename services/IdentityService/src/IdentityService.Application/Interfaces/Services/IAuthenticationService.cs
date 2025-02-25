using IdentityService.Application.Models;
using FluentResults;
namespace IdentityService.Application.Interfaces.Services;

public interface IAuthenticationService
{
    Task<Result<AuthResultDto>> RegisterUserAsync(CreateUserDto user, string password);
    Task<Result<AuthResultDto>> AuthenticateAsync(TokenRequestDto user);
    Task<Result<AuthResultDto>> RefreshTokenAsync(string refreshToken);
    Task<Result<AuthResultDto>> ConfirmEmailAsync(string email, string token);
    Task<Result> SendConfirmationEmailAsync(string email);
}
