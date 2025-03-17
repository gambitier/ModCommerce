using AccountService.Domain.Interfaces.Persistence;
using AccountService.Domain.Interfaces.Repositories;
using AccountService.Domain.Interfaces.Services;
using AccountService.Domain.Models.Users.DomainModels;

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

    public async Task CreateInitialProfileAsync(CreateUserProfileDomainModel createUserProfileDomainModel)
    {
        var existingProfile = await _userProfileRepository.GetByUserIdAsync(createUserProfileDomainModel.UserId);
        if (existingProfile != null)
        {
            // You might want to use FluentResults or similar for better error handling
            throw new InvalidOperationException($"Profile already exists for user {createUserProfileDomainModel.UserId}");
        }

        await _userProfileRepository.AddAsync(createUserProfileDomainModel);

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ConfirmEmailAsync(ConfirmUserEmailDomainModel confirmUserEmailDomainModel)
    {
        await _userProfileRepository.ConfirmEmailAsync(confirmUserEmailDomainModel);
        await _unitOfWork.SaveChangesAsync();
    }
}
