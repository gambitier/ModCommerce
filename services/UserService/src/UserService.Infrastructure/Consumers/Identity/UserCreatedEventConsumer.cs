using MassTransit;
using Microsoft.Extensions.Logging;
using UserService.Contracts.IntegrationEvents.Identity;
using UserService.Domain.Interfaces.Services;

namespace UserService.Infrastructure.Consumers.Identity;

public class UserCreatedEventConsumer : IConsumer<UserCreatedIntegrationEvent>
{
    private readonly IUserProfileService _userProfileService;
    private readonly ILogger<UserCreatedEventConsumer> _logger;

    public UserCreatedEventConsumer(
        IUserProfileService userProfileService,
        ILogger<UserCreatedEventConsumer> logger)
    {
        _userProfileService = userProfileService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserCreatedIntegrationEvent> context)
    {
        try
        {
            var message = context.Message;
            _logger.LogInformation(
                "Consuming UserCreatedEvent for user {UserId}", message.UserId);

            await _userProfileService.CreateInitialProfileAsync(
                message.UserId,
                message.Email,
                message.Username,
                message.CreatedAt);

            _logger.LogInformation(
                "Successfully created initial profile for user {UserId}", message.UserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error processing UserCreatedEvent for user {UserId}",
                context.Message.UserId);
            throw; // Let MassTransit handle the retry policy
        }
    }
}