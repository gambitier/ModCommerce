using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces.Repositories;

namespace UserService.Infrastructure.Persistence.Repositories;

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
}