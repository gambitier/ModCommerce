using AccountService.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using FluentResults;
using AccountService.Domain.Events.UserProfile;
using AccountService.Domain.Interfaces.Services;
using AccountService.Domain.Models.Users.DomainModels;

namespace AccountService.Application.EventHandlers;

public class UserEmailConfirmedIntegrationEventHandler
    : INotificationHandler<UserEmailConfirmationReceivedIntegrationEvent>
{
    private readonly ILogger<UserEmailConfirmedIntegrationEventHandler> _logger;
    private readonly IUserProfileService _userProfileService;

    public UserEmailConfirmedIntegrationEventHandler(
        IUserProfileService userProfileService,
        ILogger<UserEmailConfirmedIntegrationEventHandler> logger)
    {
        _userProfileService = userProfileService;
        _logger = logger;
    }

    public async Task Handle(
        UserEmailConfirmationReceivedIntegrationEvent notification,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await HandleNotification(notification);
            if (result.IsFailed)
            {
                var errors = string.Join("\n", result.Errors.Select(e => e.Message));
                _logger.LogError("Failed to handle {EventName}: {Errors}", nameof(UserEmailConfirmationReceivedIntegrationEvent), errors);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while handling the {EventName}", nameof(UserEmailConfirmationReceivedIntegrationEvent));
        }
    }


    private async Task<Result> HandleNotification(UserEmailConfirmationReceivedIntegrationEvent message)
    {

        _logger.LogInformation("Consuming UserEmailConfirmedEvent for user {UserId}", message.UserId);

        // Implement the logic for handling the email confirmation
        await _userProfileService.ConfirmEmailAsync(
            new ConfirmUserEmailDomainModel
            {
                UserId = message.UserId,
                Email = message.Email,
                ConfirmedAt = message.ConfirmedAt
            });

        _logger.LogInformation("Successfully confirmed email for user {UserId}", message.UserId);

        return Result.Ok();
    }
}