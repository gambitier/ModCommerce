using MassTransit;

namespace AccountService.Infrastructure.Persistence.Entities;

public class UserRegistrationSagaState : SagaStateMachineInstance
{
    /// <summary>
    /// The correlation id for the saga. 
    /// We will use the UserId to correlate the saga.
    /// </summary>
    public Guid CorrelationId { get; set; }
    public required string UserId { get; set; }
    public required string CurrentState { get; set; }
    public required string Email { get; set; }
    public bool IsProfileCreated { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
