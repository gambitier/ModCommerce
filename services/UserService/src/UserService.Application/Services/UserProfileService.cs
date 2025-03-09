using UserService.Domain.Entities;
using UserService.Domain.Interfaces.Repositories;
using UserService.Domain.Interfaces.Services;

namespace UserService.Application.Services;

public class UserProfileService : IUserProfileService
{
    private readonly IUserProfileRepository _userProfileRepository;

    public UserProfileService(IUserProfileRepository userProfileRepository)
    {
        _userProfileRepository = userProfileRepository;
    }

    public async Task CreateInitialProfileAsync(string userId, string email, string username, DateTime createdAt)
    {
        var existingProfile = await _userProfileRepository.GetByUserIdAsync(userId);
        if (existingProfile != null)
        {
            // You might want to use FluentResults or similar for better error handling
            throw new InvalidOperationException($"Profile already exists for user {userId}");
        }

        var profile = UserProfile.Create(userId, email, username, createdAt);
        await _userProfileRepository.AddAsync(profile);
    }
}