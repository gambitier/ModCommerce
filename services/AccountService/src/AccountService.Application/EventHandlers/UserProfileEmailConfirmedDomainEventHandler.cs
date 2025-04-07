using MediatR;
using Microsoft.Extensions.Logging;
using MassTransit;
using AccountService.Domain.Events.UserProfile;

namespace AccountService.Application.EventHandlers;

/// <summary>
/// Handles the <see cref="UserProfileEmailConfirmedDomainEvent"/> event.
/// </summary>
public class UserProfileEmailConfirmedDomainEventHandler : INotificationHandler<UserProfileEmailConfirmedDomainEvent>
{
    private readonly ILogger<UserProfileEmailConfirmedDomainEventHandler> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public UserProfileEmailConfirmedDomainEventHandler(
        ILogger<UserProfileEmailConfirmedDomainEventHandler> logger,
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(UserProfileEmailConfirmedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {Event} for user: {UserId}", notification.GetType().Name, notification.UserId);

        await _publishEndpoint.Publish(notification, cancellationToken);

        _logger.LogInformation("Successfully published {Event} for user: {UserId}", notification.GetType().Name, notification.UserId);
    }
}