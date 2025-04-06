using Microsoft.Extensions.Logging;
using FluentResults;
using AccountService.Domain.Interfaces.Services;
using AccountService.Domain.Models.Users.DomainModels;
using MassTransit;

namespace AccountService.Infrastructure.Sagas.UserRegistration.CommandHandlers;

public class ConfirmUserEmailCommandHandler
    : IConsumer<ConfirmUserEmailCommand>
{
    private readonly ILogger<ConfirmUserEmailCommandHandler> _logger;
    private readonly IUserProfileService _userProfileService;

    public ConfirmUserEmailCommandHandler(
        ILogger<ConfirmUserEmailCommandHandler> logger,
        IUserProfileService userProfileService
    )
    {
        _logger = logger;
        _userProfileService = userProfileService;
    }
    public async Task Consume(ConsumeContext<ConfirmUserEmailCommand> context)
    {
        var result = await HandleNotification(context.Message);
        if (result.IsFailed)
        {
            _logger.LogError("Failed to confirm user email for user {UserId}", context.Message.UserId);
        }
    }

    private async Task<Result> HandleNotification(ConfirmUserEmailCommand message)
    {

        _logger.LogInformation(
            "Consuming {CommandName} for user {UserId}",
            nameof(ConfirmUserEmailCommand),
            message.UserId
        );

        // Implement the logic for handling the email confirmation
        await _userProfileService.ConfirmEmailAsync(
            new ConfirmUserEmailDomainModel
            {
                UserId = message.UserId,
                Email = message.Email,
                ConfirmedAt = message.ConfirmedAt
            });

        _logger.LogInformation(
            "Successfully confirmed email for user {UserId}",
            message.UserId
        );

        return Result.Ok();
    }
}