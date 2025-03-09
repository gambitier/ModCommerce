namespace UserService.Domain.Entities;

public class UserProfile
{
    public Guid Id { get; private set; }
    public string UserId { get; private set; }
    public string Email { get; private set; }
    public string Username { get; private set; }
    public ProfileStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private UserProfile(string userId, string email, string username, DateTime createdAt)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Email = email;
        Username = username;
        Status = ProfileStatus.PendingVerification;
        CreatedAt = createdAt;
    }

    public static UserProfile Create(string userId, string email, string username, DateTime createdAt)
    {
        return new UserProfile(userId, email, username, createdAt);
    }

    public void Activate()
    {
        Status = ProfileStatus.Active;
    }
}

public enum ProfileStatus
{
    PendingVerification,
    Active
}