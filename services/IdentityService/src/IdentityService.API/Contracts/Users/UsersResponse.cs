namespace IdentityService.API.Contracts.Users;

public class UsersResponse
{
    public required IEnumerable<UserResponse> Users { get; set; }
}