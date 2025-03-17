using AccountService.Domain.Entities;

namespace AccountService.Domain.Interfaces.Repositories;

public interface IUserProfileRepository
{
    Task AddAsync(UserProfile userProfile);
    Task<UserProfile?> GetByUserIdAsync(string userId);
    Task ConfirmEmailAsync(string email);
}
