using Microsoft.EntityFrameworkCore;
using AccountService.Domain.Entities;
using AccountService.Domain.Interfaces.Repositories;

namespace AccountService.Infrastructure.Persistence.Repositories;

public class UserProfileRepository : IUserProfileRepository
{
    private readonly UserServiceDbContext _dbContext;

    public UserProfileRepository(UserServiceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(UserProfile userProfile)
    {
        await _dbContext.UserProfiles.AddAsync(userProfile);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<UserProfile?> GetByUserIdAsync(string userId)
    {
        return await _dbContext.UserProfiles
            .FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task ConfirmEmailAsync(string email)
    {
        var profile = await _dbContext.UserProfiles
            .FirstOrDefaultAsync(x => x.Email == email);

        if (profile == null)
        {
            throw new InvalidOperationException($"Profile not found for email {email}");
        }

        profile.Activate();
    }
}