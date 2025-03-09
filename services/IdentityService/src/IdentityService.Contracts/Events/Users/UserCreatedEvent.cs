using System.Text.Json.Serialization;

namespace IdentityService.Contracts.Events.Users;

/// <summary>
/// Event published when a new user is created in the Identity service.
/// </summary>
/// <param name="UserId">The unique identifier of the created user.</param>
/// <param name="Email">The email address of the created user.</param>
/// <param name="Username">The username of the created user.</param>
/// <param name="CreatedAt">The UTC timestamp when the user was created.</param>
public record UserCreatedEvent(
    /// <summary>
    /// The unique identifier of the created user.
    /// </summary>
    [property: JsonPropertyName("userId")]
    string UserId,

    /// <summary>
    /// The email address of the created user.
    /// </summary>
    [property: JsonPropertyName("email")]
    string Email,

    /// <summary>
    /// The username of the created user.
    /// </summary>
    [property: JsonPropertyName("username")]
    string Username,

    /// <summary>
    /// The UTC timestamp when the user was created.
    /// </summary>
    [property: JsonPropertyName("createdAt")]
    DateTime CreatedAt
);
