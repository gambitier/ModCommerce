namespace IdentityService.Contracts.API.Users.Responses;

public class UsersResponse
{
    public required IEnumerable<UserResponse> Users { get; set; }
}