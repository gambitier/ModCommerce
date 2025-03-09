using MediatR;

namespace IdentityService.Domain.Events;

public class UserCreatedDomainEvent : INotification
{
    public string UserId { get; }
    public string Email { get; }
    public string Username { get; }
    public DateTime CreatedAt { get; }

    public UserCreatedDomainEvent(string userId, string email, string username, DateTime createdAt)
    {
        UserId = userId;
        Email = email;
        Username = username;
        CreatedAt = createdAt;
    }
}
