using MediatR;

namespace IdentityService.Domain.Events;

public class UserCreatedDomainEvent : INotification
{
    public string UserId { get; }
    public string Email { get; }
    public string Username { get; }

    public UserCreatedDomainEvent(string userId, string email, string username)
    {
        UserId = userId;
        Email = email;
        Username = username;
    }
}
