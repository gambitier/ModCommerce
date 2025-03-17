using MassTransit;
using Microsoft.Extensions.Logging;
using AccountService.Domain.Interfaces.Services;
using AccountService.Domain.Models.Users.DomainModels;

namespace AccountService.Infrastructure.MessageQueue.IdentityService.Events.UserCreated;

/// <summary>
/// This is a consumer for the <see cref="UserCreatedEvent"/>.
/// It is used to create an initial profile for a user.
/// </summary>
public class UserCreatedEventConsumer : IConsumer<UserCreatedEvent>
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

    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        try
        {
            var message = context.Message;
            _logger.LogInformation(
                "Consuming UserCreatedEvent for user {UserId}", message.UserId);

            await _userProfileService.CreateInitialProfileAsync(
                new CreateUserProfileDomainModel
                {
                    UserId = message.UserId,
                    Email = message.Email,
                    Username = message.Username,
                    CreatedAt = message.CreatedAt
                });

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
