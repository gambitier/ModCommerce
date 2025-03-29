using IdentityService.Domain.Interfaces.Events;
using MediatR;

namespace IdentityService.Infrastructure.Events;

/// <summary>
/// Publishes domain events.
/// </summary>
public class DomainEventPublisher : IDomainEventPublisher
{
    private readonly IMediator _mediator;

    public DomainEventPublisher(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <inheritdoc />
    public async Task PublishAsync(object domainEvent)
    {
        await _mediator.Publish(domainEvent);
    }
}
