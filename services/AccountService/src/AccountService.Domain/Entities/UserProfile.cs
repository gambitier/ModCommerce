namespace AccountService.Domain.Entities;

/// <summary>
/// Represents a user's profile information database entity.
/// </summary>
public class UserProfile
{
    /// <summary>
    /// The unique identifier of the user profile.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// The unique identifier of the user created in the Identity service.
    /// </summary>
    public string UserId { get; private set; }

    /// <summary>
    /// The email address of the user, provided by the Identity service.
    /// </summary>
    public string Email { get; private set; }

    /// <summary>
    /// The username of the user, provided by the Identity service.
    /// </summary>
    public string Username { get; private set; }

    /// <summary>
    /// The status of the user profile.
    /// </summary>
    public ProfileStatus Status { get; private set; }

    /// <summary>
    /// The date and time the user profile was created.
    /// </summary>
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

    /// <summary>
    /// Creates a new user profile.
    /// </summary>
    public static UserProfile Create(
        string userId,
        string email,
        string username,
        DateTime createdAt)
    {
        return new UserProfile(userId, email, username, createdAt);
    }

    /// <summary>
    /// Activates the user profile.
    /// Should only be called when the user's email is confirmed.
    /// </summary>
    public void Activate()
    {
        Status = ProfileStatus.Active;
    }
}

/// <summary>
/// The status of the user profile.
/// </summary>
public enum ProfileStatus
{
    PendingVerification,
    Active
}