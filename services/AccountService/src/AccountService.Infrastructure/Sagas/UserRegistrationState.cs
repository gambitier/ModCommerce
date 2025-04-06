using MassTransit;

namespace AccountService.Infrastructure.Sagas;

/// <summary>
/// Represents the state of a user registration saga.
/// </summary>
public class UserRegistrationState : SagaStateMachineInstance
{
    /// <summary>
    /// Gets or sets the correlation ID.
    /// </summary>
    public Guid CorrelationId { get; set; }

    /// <summary>
    /// Gets or sets the current state.
    /// </summary>
    public string CurrentState { get; set; } = null!;

    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    public string UserId { get; set; } = null!;

    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    public string Email { get; set; } = null!;

    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the email confirmation date.
    /// </summary>
    public DateTime? EmailConfirmedAt { get; set; }
}