using MassTransit;
using Microsoft.Extensions.Logging;
using AccountService.Domain.Interfaces.Services;
using AccountService.Domain.Models.Users.DomainModels;

namespace AccountService.Infrastructure.Sagas.Commands;

/// <summary>
/// Consumer for the <see cref="ConfirmUserEmailCommand"/>.
/// </summary>
public class ConfirmUserEmailCommandConsumer : IConsumer<ConfirmUserEmailCommand>
{
    private readonly IUserProfileService _userProfileService;
    private readonly ILogger<ConfirmUserEmailCommandConsumer> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfirmUserEmailCommandConsumer"/> class.
    /// </summary>
    /// <param name="userProfileService">The user profile service.</param>
    /// <param name="logger">The logger.</param>
    public ConfirmUserEmailCommandConsumer(
        IUserProfileService userProfileService,
        ILogger<ConfirmUserEmailCommandConsumer> logger)
    {
        _userProfileService = userProfileService;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task Consume(ConsumeContext<ConfirmUserEmailCommand> context)
    {
        try
        {
            var command = context.Message;
            _logger.LogInformation(
                "Consuming ConfirmUserEmailCommand for user {UserId}", command.UserId);

            await _userProfileService.ConfirmEmailAsync(
                new ConfirmUserEmailDomainModel
                {
                    UserId = command.UserId,
                    Email = command.Email,
                    ConfirmedAt = command.ConfirmedAt
                });

            _logger.LogInformation(
                "Successfully confirmed email for user {UserId}", command.UserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error processing ConfirmUserEmailCommand for user {UserId}",
                context.Message.UserId);
            throw; // Let MassTransit handle the retry policy
        }
    }
}