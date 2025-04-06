using MediatR;

namespace AccountService.Domain.Events.UserProfile;

public class UserEmailConfirmationReceivedIntegrationEvent : INotification
{
    public required string UserId { get; set; }
    public required string Email { get; set; }
    public required DateTime ConfirmedAt { get; set; }
}