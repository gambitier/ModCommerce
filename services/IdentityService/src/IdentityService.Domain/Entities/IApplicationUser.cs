namespace IdentityService.Domain.Entities;

public interface IApplicationUser
{
    string Id { get; }
    string? Email { get; }
}
