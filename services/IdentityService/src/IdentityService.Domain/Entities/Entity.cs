using IdentityService.Domain.Interfaces.Events;

namespace IdentityService.Domain.Entities;

/// <summary>
/// Base class for all entities.
/// Use this class to add domain events to an entity.
/// If it's not possible to inherit from this class, use the <see cref="IHasDomainEvents"/> interface instead.
/// </summary>
public abstract class Entity : IHasDomainEvents
{
    private readonly List<object> _domainEvents = new();

    /// <inheritdoc />
    public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

    /// <inheritdoc />
    public void AddDomainEvent(object domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <inheritdoc />
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
