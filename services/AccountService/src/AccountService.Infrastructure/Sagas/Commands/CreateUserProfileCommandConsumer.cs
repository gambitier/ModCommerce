using MassTransit;
using Microsoft.Extensions.Logging;
using AccountService.Domain.Interfaces.Services;
using AccountService.Domain.Models.Users.DomainModels;

namespace AccountService.Infrastructure.Sagas.Commands;

/// <summary>
/// Consumer for the <see cref="CreateUserProfileCommand"/>.
/// </summary>
public class CreateUserProfileCommandConsumer : IConsumer<CreateUserProfileCommand>
{
    private readonly IUserProfileService _userProfileService;
    private readonly ILogger<CreateUserProfileCommandConsumer> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserProfileCommandConsumer"/> class.
    /// </summary>
    /// <param name="userProfileService">The user profile service.</param>
    /// <param name="logger">The logger.</param>
    public CreateUserProfileCommandConsumer(
        IUserProfileService userProfileService,
        ILogger<CreateUserProfileCommandConsumer> logger)
    {
        _userProfileService = userProfileService;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task Consume(ConsumeContext<CreateUserProfileCommand> context)
    {
        try
        {
            var command = context.Message;
            _logger.LogInformation(
                "Consuming CreateUserProfileCommand for user {UserId}", command.UserId);

            await _userProfileService.CreateInitialProfileAsync(
                new CreateUserProfileDomainModel
                {
                    UserId = command.UserId,
                    Email = command.Email,
                    Username = command.Username,
                    CreatedAt = command.CreatedAt
                });

            _logger.LogInformation(
                "Successfully created initial profile for user {UserId}", command.UserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error processing CreateUserProfileCommand for user {UserId}",
                context.Message.UserId);
            throw; // Let MassTransit handle the retry policy
        }
    }
}