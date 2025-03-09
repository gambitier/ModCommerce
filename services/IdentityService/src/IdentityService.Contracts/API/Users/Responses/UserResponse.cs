namespace IdentityService.Contracts.API.Users.Responses;

/// <summary>
/// A response representing a user
/// </summary>
/// <param name="Id">The user's ID</param>
/// <param name="Email">The user's email</param>
/// <param name="CreatedAt">The date and time the user was created</param>
public record UserResponse(
    string Id,
    string Email,
    DateTime CreatedAt
);
