using IdentityService.Domain.Entities;

namespace IdentityService.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<IApplicationUser?> FindByEmailAsync(string email);
    Task<bool> CheckPasswordAsync(IApplicationUser user, string password);
    Task<(bool Succeeded, string[] Errors)> CreateAsync(string email, string password, string? firstName = null, string? lastName = null);
}
