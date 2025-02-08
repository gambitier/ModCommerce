using FluentResults;
using IdentityService.Domain.Models;

namespace IdentityService.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<Result<UserDomainModel>> CreateAsync(string email, string password);
    Task<Result<bool>> CheckPasswordAsync(string userId, string password);
    Task<Result<UserDomainModel>> FindByEmailAsync(string email);
}
