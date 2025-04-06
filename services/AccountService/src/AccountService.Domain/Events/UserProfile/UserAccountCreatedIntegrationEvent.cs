using MediatR;

namespace AccountService.Domain.Events.UserProfile;

public class UserAccountCreatedIntegrationEvent : INotification
{
    public required string UserId { get; set; }
    public required string Email { get; set; }
    public required string Username { get; set; }
    public required DateTime CreatedAt { get; set; }
}