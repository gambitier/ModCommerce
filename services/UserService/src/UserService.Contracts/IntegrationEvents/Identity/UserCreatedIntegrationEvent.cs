namespace UserService.Contracts.IntegrationEvents.Identity;

/// <summary>
/// Integration event raised by the Identity service when a new user is created.
/// This event is consumed by the User service to maintain user data consistency.
/// </summary>
public class UserCreatedIntegrationEvent
{
    public required string UserId { get; init; }
    public required string Email { get; init; }
    public required string Username { get; init; }
    public required DateTime CreatedAt { get; init; }
}