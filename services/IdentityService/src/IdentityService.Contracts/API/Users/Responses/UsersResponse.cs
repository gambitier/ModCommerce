namespace IdentityService.Contracts.API.Users.Responses;

/// <summary>
/// A response representing a list of users
/// </summary>
/// <param name="Users">The list of users</param>
public record UsersResponse(
    /// <summary>
    /// The list of users
    /// </summary>
    IEnumerable<UserResponse> Users
);