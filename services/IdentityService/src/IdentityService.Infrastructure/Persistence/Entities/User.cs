using IdentityService.Domain.Events;
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
        var user = new IdentityUser
        {
            UserName = username,
            Email = email,
        };

        user.AddDomainEvent(new UserCreatedDomainEvent(user.Id, user.Email, user.UserName));

        return user;
    }
}
