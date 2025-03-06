namespace IdentityService.Domain.Models;

public class User
{
    public required string Id { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required bool EmailConfirmed { get; set; }
}
