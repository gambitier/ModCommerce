using IdentityService.Application.Models;
using FluentResults;
using IdentityService.Domain.Models;
namespace IdentityService.Application.Interfaces.Services;

public interface IAuthenticationService
{
    Task<Result> RegisterUserAsync(CreateUserDto user, string password);
    Task<Result<AuthResultDto>> AuthenticateAsync(TokenRequestDto user);
    Task<Result<AuthResultDto>> RefreshTokenAsync(string refreshToken);
    Task<Result<AuthResultDto>> ConfirmEmailAsync(string email, string token);
    Task<Result> SendConfirmationEmailAsync(string email);
    JsonWebKeyInfo GetJsonWebKey();
}
