using MediatR;
using IdentityService.Domain.Events;
using IdentityService.Application.Interfaces.Services;

public class UserCreatedDomainEventHandler : INotificationHandler<UserCreatedDomainEvent>
{
    private readonly IAuthenticationService _authenticationService;

    public UserCreatedDomainEventHandler(
        IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var confirmationTokenResult = await _authenticationService.SendConfirmationEmailAsync(notification.Email);
        if (confirmationTokenResult.IsFailed)
            throw new Exception("Failed to generate confirmation token");
    }
}