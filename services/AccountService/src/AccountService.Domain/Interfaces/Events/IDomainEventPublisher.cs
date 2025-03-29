namespace AccountService.Domain.Interfaces.Events;

/// <summary>
/// Publishes domain events.
/// </summary>
public interface IDomainEventPublisher
{
    /// <summary>
    /// Publishes a domain event.
    /// </summary>
    /// <param name="domainEvent">The domain event to publish.</param>
    Task PublishAsync(object domainEvent);
}
