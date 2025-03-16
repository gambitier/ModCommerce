using MediatR;
using Microsoft.Extensions.Logging;
using IdentityService.Domain.Events;
using MassTransit;
using IdentityService.Contracts.Events.Users;

namespace IdentityService.Application.EventHandlers;
/// <summary>
/// Handles the <see cref="UserEmailConfirmedDomainEvent"/> event.
/// </summary>
public class UserEmailConfirmedDomainEventHandler : INotificationHandler<UserEmailConfirmedDomainEvent>
{
    private readonly ILogger<UserEmailConfirmedDomainEventHandler> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public UserEmailConfirmedDomainEventHandler(
        ILogger<UserEmailConfirmedDomainEventHandler> logger,
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(UserEmailConfirmedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling user email confirmed event for user: {UserId}", notification.UserId);

        await _publishEndpoint.Publish(
            new UserEmailConfirmedEvent(
                notification.UserId,
                notification.Email,
                notification.ConfirmedAt
            ),
            cancellationToken
        );

        _logger.LogInformation("Successfully published user email confirmed event for user: {UserId}", notification.UserId);
    }
}