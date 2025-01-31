namespace IdentityService.Domain.Entities;

public interface IApplicationUser
{
    string Id { get; }
    string? Email { get; }
    string? FirstName { get; }
    string? LastName { get; }
    DateTime CreatedAt { get; }
    DateTime UpdatedAt { get; }
}
