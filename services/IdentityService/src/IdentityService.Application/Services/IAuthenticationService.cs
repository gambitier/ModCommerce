using IdentityService.Domain.Entities;

namespace IdentityService.Application.Services;

public interface IAuthenticationService
{
    Task<AuthResult> RegisterUserAsync(DomainUser user, string password);
    Task<AuthResult> AuthenticateAsync(string email, string password);
}

public class AuthResult
{
    public bool Succeeded { get; set; }
    public string Token { get; set; }
    public IEnumerable<string> Errors { get; set; } = Array.Empty<string>();
}