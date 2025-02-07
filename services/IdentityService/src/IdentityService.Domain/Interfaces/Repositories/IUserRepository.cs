using IdentityService.Domain.Entities;
using FluentResults;
namespace IdentityService.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<Result<(string UserId, string Email)>> CreateAsync(string email, string password);
    Task<Result<bool>> CheckPasswordAsync(string userId, string password);
    Task<Result<IApplicationUser>> FindByEmailAsync(string email);
}
