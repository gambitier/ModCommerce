using AccountService.Domain.Entities;
using AccountService.Domain.Interfaces.Persistence;
using AccountService.Domain.Interfaces.Repositories;
using AccountService.Domain.Interfaces.Services;

namespace AccountService.Application.Services;

public class UserProfileService : IUserProfileService
{
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UserProfileService(
        IUserProfileRepository userProfileRepository,
        IUnitOfWork unitOfWork)
    {
        _userProfileRepository = userProfileRepository;
        _unitOfWork = unitOfWork;
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

    public async Task ConfirmEmailAsync(string userId, string email, DateTime confirmedAt)
    {
        await _userProfileRepository.ConfirmEmailAsync(email);
        await _unitOfWork.SaveChangesAsync();
    }
}
