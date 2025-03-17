using AccountService.Domain.Models.Users.DomainModels;
namespace AccountService.Domain.Interfaces.Services;

public interface IUserProfileService
{
    Task CreateInitialProfileAsync(CreateUserProfileDomainModel createUserProfileDomainModel);
    Task ConfirmEmailAsync(ConfirmUserEmailDomainModel confirmUserEmailDomainModel);
}
