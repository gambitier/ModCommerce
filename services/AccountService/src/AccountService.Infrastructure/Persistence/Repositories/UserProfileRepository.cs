using Microsoft.EntityFrameworkCore;
using AccountService.Domain.Interfaces.Repositories;
using AccountService.Domain.Models.Users.Dtos;
using AccountService.Infrastructure.Persistence.Entities;
using AccountService.Domain.Models.Users.DomainModels;

namespace AccountService.Infrastructure.Persistence.Repositories;

public class UserProfileRepository : IUserProfileRepository
{
    private readonly AccountDbContext _dbContext;

    public UserProfileRepository(AccountDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(CreateUserProfileDomainModel createUserProfileDomainModel)
    {
        var profile = UserProfileEntity.Create(createUserProfileDomainModel);
        await _dbContext.UserProfiles.AddAsync(profile);
    }

    public async Task<UserProfileDto?> GetByUserIdAsync(string userId)
    {
        var query = _dbContext.UserProfiles
            .Where(x => x.UserId == userId)
            .Select(x => new UserProfileDto
            {
                Id = x.Id,
                UserId = x.UserId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Username = x.Username,
                CreatedAt = x.CreatedAt
            });

        return await query.FirstOrDefaultAsync();
    }

    public async Task<Dictionary<string, UserProfileDto?>> GetUserProfilesAsync(IEnumerable<string> userIds)
    {
        var distinctUserIds = userIds.Distinct().ToList();

        var query = _dbContext.UserProfiles
            .Where(x => distinctUserIds.Contains(x.UserId))
            .Select(x => new UserProfileDto
            {
                Id = x.Id,
                UserId = x.UserId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Username = x.Username,
                CreatedAt = x.CreatedAt
            });

        var foundProfiles = await query.ToDictionaryAsync(x => x.UserId, x => x);

        return distinctUserIds.ToDictionary(
            userId => userId,
            userId => foundProfiles.GetValueOrDefault(userId)
        );
    }

    public async Task ConfirmEmailAsync(ConfirmUserEmailDomainModel confirmUserEmailDomainModel)
    {
        var profile = await _dbContext.UserProfiles
            .FirstOrDefaultAsync(x => x.Email == confirmUserEmailDomainModel.Email)
            ?? throw new InvalidOperationException($"Profile not found for email {confirmUserEmailDomainModel.Email}");

        profile.Activate();
    }

}