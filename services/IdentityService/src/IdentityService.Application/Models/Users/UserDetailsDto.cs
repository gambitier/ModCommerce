namespace IdentityService.Application.Models.Users;

public class UserDetailsDto
{
    public required string Id { get; init; }
    public required string Email { get; init; }
    public required DateTime CreatedAt { get; init; }
}
