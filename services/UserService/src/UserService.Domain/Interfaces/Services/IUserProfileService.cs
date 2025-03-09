namespace UserService.Domain.Interfaces.Services;

public interface IUserProfileService
{
    Task CreateInitialProfileAsync(string userId, string email, string username, DateTime createdAt);
}
