namespace IdentityService.Contracts.Events.Users;

public record UserCreatedEvent(
    string UserId,
    string Email,
    string Username,
    DateTime CreatedAt
);
