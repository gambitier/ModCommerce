using MassTransit;
using Microsoft.Extensions.Logging;
using AccountService.Domain.Interfaces.Events;
using AccountService.Domain.Events.UserProfile;
namespace AccountService.Infrastructure.MessageQueue.IdentityService.Events.UserEmailConfirmed;

/// <summary>
/// This is a consumer for the <see cref="UserEmailConfirmedEvent"/>.
/// It is used to handle the event when a user's email is confirmed.
/// </summary>
public class UserEmailConfirmedEventConsumer : IConsumer<UserEmailConfirmedEvent>
{
    private readonly IDomainEventPublisher _domainEventPublisher;
    private readonly ILogger<UserEmailConfirmedEventConsumer> _logger;
    public UserEmailConfirmedEventConsumer(
        IDomainEventPublisher domainEventPublisher,
        ILogger<UserEmailConfirmedEventConsumer> logger)
    {
        _domainEventPublisher = domainEventPublisher;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserEmailConfirmedEvent> context)
    {
        try
        {
            var message = context.Message;
            await _domainEventPublisher.PublishAsync(
                new UserEmailConfirmationReceivedIntegrationEvent
                {
                    UserId = message.UserId,
                    Email = message.Email,
                    ConfirmedAt = message.ConfirmedAt
                }
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error processing {EventName} for user {UserId}",
                nameof(UserEmailConfirmedEvent),
                context.Message.UserId);
            throw; // Let MassTransit handle the retry policy
        }
    }
}
