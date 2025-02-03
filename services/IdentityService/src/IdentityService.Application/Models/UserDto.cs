namespace IdentityService.Application.Models;

public class UserDto
{
    public required string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
