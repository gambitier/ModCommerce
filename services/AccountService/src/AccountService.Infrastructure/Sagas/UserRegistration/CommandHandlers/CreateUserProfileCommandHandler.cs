using Microsoft.Extensions.Logging;
using FluentResults;
using AccountService.Domain.Interfaces.Services;
using AccountService.Domain.Models.Users.DomainModels;
using MassTransit;

namespace AccountService.Infrastructure.Sagas.UserRegistration.CommandHandlers;

public class CreateUserProfileCommandHandler
    : IConsumer<CreateUserProfileCommand>
{
    private readonly ILogger<CreateUserProfileCommandHandler> _logger;
    private readonly IUserProfileService _userProfileService;

    public CreateUserProfileCommandHandler(
        ILogger<CreateUserProfileCommandHandler> logger,
        IUserProfileService userProfileService
    )
    {
        _logger = logger;
        _userProfileService = userProfileService;
    }

    public async Task Consume(ConsumeContext<CreateUserProfileCommand> context)
    {
        var result = await HandleNotification(context.Message);
        if (result.IsFailed)
        {
            _logger.LogError("Failed to confirm user email for user {UserId}", context.Message.UserId);
        }
    }

    private async Task<Result> HandleNotification(CreateUserProfileCommand message)
    {
        _logger.LogInformation(
            "Consuming {CommandName} for user {UserId}",
            nameof(CreateUserProfileCommand),
            message.UserId);

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

        return Result.Ok();
    }
}