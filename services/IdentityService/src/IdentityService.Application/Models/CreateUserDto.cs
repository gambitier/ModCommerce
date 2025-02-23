namespace IdentityService.Application.Models;

public class CreateUserDto
{
    public required string Username { get; init; }
    public required string Email { get; init; }
}
