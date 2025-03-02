using MediatR;
using Microsoft.Extensions.Logging;
using IdentityService.Domain.Events;
using IdentityService.Application.Interfaces.Services;

public class UserCreatedDomainEventHandler : INotificationHandler<UserCreatedDomainEvent>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<UserCreatedDomainEventHandler> _logger;

    public UserCreatedDomainEventHandler(
        IAuthenticationService authenticationService,
        ILogger<UserCreatedDomainEventHandler> logger)
    {
        _authenticationService = authenticationService;
        _logger = logger;
    }

    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            var confirmationTokenResult = await _authenticationService.SendConfirmationEmailAsync(notification.Email);
            if (confirmationTokenResult.IsFailed)
            {
                _logger.LogError(
                    "Failed to send confirmation email for user with email {Email}. Errors: {Errors}",
                    notification.Email,
                    string.Join(", ", confirmationTokenResult.Errors));

                // TODO: Implement retry mechanism
                return;
            }

            _logger.LogInformation(
                "Successfully sent confirmation email to {Email}",
                notification.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "An unexpected error occurred while sending confirmation email to {Email}",
                notification.Email);

            // TODO: Implement retry logic
        }
    }
}