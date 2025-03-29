namespace IdentityService.Domain.Interfaces.Events;

/// <summary>
/// Use this interface to add domain events to an entity.
/// If it's not possible to inherit from the <see cref="Entities.Entity"/> class, use this interface instead.
/// </summary>
public interface IHasDomainEvents
{
    /// <summary>
    /// Gets the domain events that have been added to the entity.
    /// </summary>
    IReadOnlyCollection<object> DomainEvents { get; }

    /// <summary>
    /// Adds a domain event to the entity.
    /// </summary>
    /// <param name="domainEvent">The domain event to add.</param>
    void AddDomainEvent(object domainEvent);

    /// <summary>
    /// Clears the domain events that have been added to the entity.
    /// </summary>
    void ClearDomainEvents();
}
