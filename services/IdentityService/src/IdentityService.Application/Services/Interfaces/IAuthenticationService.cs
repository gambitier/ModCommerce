using IdentityService.Domain.Entities;
using IdentityService.Application.Models;

namespace IdentityService.Application.Services.Interfaces;

public interface IAuthenticationService
{
    Task<AuthResultDto> RegisterUserAsync(DomainUser user, string password);
    Task<AuthResultDto> AuthenticateAsync(string email, string password);
}
