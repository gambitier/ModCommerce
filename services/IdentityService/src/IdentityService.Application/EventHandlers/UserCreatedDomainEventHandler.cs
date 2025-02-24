using MediatR;
using IdentityService.Domain.Interfaces.Communication;
using IdentityService.Domain.Events;

public class UserCreatedDomainEventHandler : INotificationHandler<UserCreatedDomainEvent>
{
    private readonly IEmailService _emailService;

    public UserCreatedDomainEventHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Generate confirmation link (this should come from a service)
        var confirmationLink = $"https://your-domain.com/confirm-email?userId={notification.UserId}&token={GenerateToken()}";

        await _emailService.SendConfirmationEmailAsync(
            notification.Email,
            notification.Username,
            confirmationLink
        );
    }

    private string GenerateToken()
    {
        // Implement token generation logic
        return Guid.NewGuid().ToString();
    }
}