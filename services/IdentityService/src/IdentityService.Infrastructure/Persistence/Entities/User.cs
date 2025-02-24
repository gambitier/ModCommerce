using IdentityService.Domain.Interfaces.Events;

namespace IdentityService.Infrastructure.Persistence.Entities;

public class IdentityUser : Microsoft.AspNetCore.Identity.IdentityUser, IHasDomainEvents
{
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
        return new IdentityUser
        {
            UserName = username,
            Email = email,
        };
    }
}
