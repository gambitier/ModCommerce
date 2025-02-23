namespace IdentityService.Application.Models;

public class TokenRequestDto
{
    public required string UsernameOrEmail { get; init; }
    public required string Password { get; init; }
}
