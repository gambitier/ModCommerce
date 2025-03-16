using MediatR;

namespace IdentityService.Domain.Events;

public class UserEmailConfirmedDomainEvent : INotification
{
    public string UserId { get; }
    public string Email { get; }
    public DateTime ConfirmedAt { get; }

    public UserEmailConfirmedDomainEvent(string userId, string email, DateTime confirmedAt)
    {
        UserId = userId;
        Email = email;
        ConfirmedAt = confirmedAt;
    }
}
