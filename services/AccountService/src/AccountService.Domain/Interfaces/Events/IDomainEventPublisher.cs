namespace AccountService.Domain.Interfaces.Events;

public interface IDomainEventPublisher
{
    Task PublishAsync(object domainEvent);
}
