using AccountService.Domain.Models.Users.DomainModels;

namespace AccountService.Infrastructure.Persistence.Entities;

/// <summary>
/// Represents a user's profile information database entity.
/// </summary>
public class UserProfileEntity
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
    /// The first name of the user.
    /// </summary>
    public string? FirstName { get; private set; }

    /// <summary>
    /// The last name of the user.
    /// </summary>
    public string? LastName { get; private set; }

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

    /// <summary>
    /// The date and time the user profile was updated.
    /// </summary>
    public DateTime? UpdatedAt { get; private set; }

    private UserProfileEntity(
        string userId,
        string email,
        string username,
        DateTime createdAt)
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
    public static UserProfileEntity Create(CreateUserProfileDomainModel createUserProfileDomainModel)
    {
        var (UserId, Email, Username, CreatedAt) = createUserProfileDomainModel;
        return new UserProfileEntity(UserId, Email, Username, CreatedAt);
    }

    /// <summary>
    /// Activates the user profile.
    /// Should only be called when the user's email is confirmed.
    /// </summary>
    public void Activate()
    {
        Status = ProfileStatus.Active;
    }

    public void Update(UpdateUserProfileDomainModel updateUserProfileDomainModel)
    {
        var (firstName, lastName) = updateUserProfileDomainModel;
        FirstName = firstName;
        LastName = lastName;
        UpdatedAt = DateTime.UtcNow;
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