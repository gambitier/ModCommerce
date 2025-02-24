using IdentityService.Domain.Interfaces.Events;
using MediatR;

namespace IdentityService.Infrastructure.Events;

public class DomainEventPublisher : IDomainEventPublisher
{
    private readonly IMediator _mediator;

    public DomainEventPublisher(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task PublishAsync(object domainEvent)
    {
        await _mediator.Publish(domainEvent);
    }
}
