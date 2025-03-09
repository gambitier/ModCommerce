namespace IdentityService.Contracts.API.Users.Responses;

public class UserResponse
{
    public required string Id { get; set; }
    public required string Email { get; set; }
    public DateTime CreatedAt { get; set; }
}