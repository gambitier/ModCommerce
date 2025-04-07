using MediatR;
using Microsoft.Extensions.Logging;
using MassTransit;
using AccountService.Domain.Events.UserProfile;

namespace AccountService.Application.EventHandlers;

/// <summary>
/// Handles the <see cref="UserProfileCreatedDomainEvent"/> event.
/// </summary>
public class UserProfileCreatedDomainEventHandler : INotificationHandler<UserProfileCreatedDomainEvent>
{
    private readonly ILogger<UserProfileCreatedDomainEventHandler> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public UserProfileCreatedDomainEventHandler(
        ILogger<UserProfileCreatedDomainEventHandler> logger,
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(UserProfileCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {Event} for user: {UserId}", notification.GetType().Name, notification.UserId);

        await _publishEndpoint.Publish(notification, cancellationToken);

        _logger.LogInformation("Successfully published {Event} for user: {UserId}", notification.GetType().Name, notification.UserId);
    }
}