using IdentityService.Domain.Events;
using IdentityService.Domain.Interfaces.Events;

namespace IdentityService.Infrastructure.Persistence.Entities;

public class IdentityUser : Microsoft.AspNetCore.Identity.IdentityUser, IHasDomainEvents
{
    /// <summary>
    /// The date and time the user was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    private readonly List<object> _domainEvents = [];
    public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(object domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public static IdentityUser Create(string username, string email)
    {
        var user = new IdentityUser
        {
            UserName = username,
            Email = email,
            CreatedAt = DateTime.UtcNow
        };

        user.AddDomainEvent(new UserCreatedDomainEvent(
            user.Id,
            user.Email,
            user.UserName,
            user.CreatedAt
        ));

        return user;
    }

    public void ConfirmEmail()
    {
        EmailConfirmed = true;
        AddDomainEvent(new UserEmailConfirmedDomainEvent(
            Id,
            Email!,
            DateTime.UtcNow
        ));
    }
}
