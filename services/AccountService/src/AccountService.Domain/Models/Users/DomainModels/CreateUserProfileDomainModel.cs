namespace AccountService.Domain.Models.Users.DomainModels;

public class CreateUserProfileDomainModel
{
    public required string UserId { get; init; }
    public required string Email { get; init; }
    public required string Username { get; init; }
    public required DateTime CreatedAt { get; init; }

    public void Deconstruct(
        out string userId,
        out string email,
        out string username,
        out DateTime createdAt)
    {
        userId = UserId;
        email = Email;
        username = Username;
        createdAt = CreatedAt;
    }
}
