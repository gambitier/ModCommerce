using MassTransit;
using Microsoft.Extensions.Logging;
using AccountService.Domain.Events.UserProfile;
using AccountService.Domain.Interfaces.Events;
namespace AccountService.Infrastructure.MessageQueue.IdentityService.Events.UserCreated;

/// <summary>
/// This is a consumer for the <see cref="UserCreatedEvent"/>.
/// It is used to create an initial profile for a user.
/// </summary>
public class UserCreatedEventConsumer : IConsumer<UserCreatedEvent>
{
    private readonly IDomainEventPublisher _domainEventPublisher;
    private readonly ILogger<UserCreatedEventConsumer> _logger;

    public UserCreatedEventConsumer(
        IDomainEventPublisher domainEventPublisher,
        ILogger<UserCreatedEventConsumer> logger)
    {
        _domainEventPublisher = domainEventPublisher;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        try
        {
            await _domainEventPublisher.PublishAsync(
                new UserAccountCreatedIntegrationEvent
                {
                    UserId = context.Message.UserId,
                    Email = context.Message.Email,
                    Username = context.Message.Username,
                    CreatedAt = context.Message.CreatedAt
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error processing {EventName} for user {UserId}",
                nameof(UserCreatedEvent),
                context.Message.UserId);
            throw; // Let MassTransit handle the retry policy
        }
    }
}
