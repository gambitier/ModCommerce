namespace AccountService.Domain.Models.Users.Dtos;

/// <summary>
/// Represents a user's profile information.
/// </summary>
public class UserProfileDto
{
    /// <summary>
    /// The unique identifier of the user profile.
    /// </summary>
    public required Guid Id { get; init; }

    /// <summary>
    /// The unique identifier of the user created in the Identity service.
    /// </summary>
    public required string UserId { get; init; }

    /// <summary>
    /// The email address of the user, provided by the Identity service.
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// The username of the user, provided by the Identity service.
    /// </summary>
    public required string Username { get; init; }

    /// <summary>
    /// The date and time the user profile was created.
    /// </summary>
    public required DateTime CreatedAt { get; init; }
}
