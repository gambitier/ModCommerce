using MassTransit;
using Microsoft.Extensions.Logging;
using AccountService.Infrastructure.MessageQueue.IdentityService.Events.UserCreated;
using AccountService.Infrastructure.MessageQueue.IdentityService.Events.UserEmailConfirmed;
using AccountService.Domain.Utils;
using AccountService.Infrastructure.Sagas.Commands;
using AccountService.Domain.Events.UserProfile;

namespace AccountService.Infrastructure.Sagas;

/// <summary>
/// State machine for handling user registration saga.
/// </summary>
public class UserRegistrationStateMachine : MassTransitStateMachine<UserRegistrationState>
{
    private readonly ILogger<UserRegistrationStateMachine> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRegistrationStateMachine"/> class.
    /// </summary>
    /// <param name="logger">The logger.</param>
    public UserRegistrationStateMachine(
        ILogger<UserRegistrationStateMachine> logger)
    {
        _logger = logger;

        // Configure the state machine
        InstanceState(x => x.CurrentState);

        // Define events with correlation
        ConfigureEvents();

        // Configure state transitions
        ConfigureStateTransitions();

        SetCompletedWhenFinalized();
    }

    private void ConfigureEvents()
    {
        Event(() => UserCreated, x => x.CorrelateById(context =>
            GuidUtils.CreateDeterministicGuid(context.Message.UserId)));

        Event(() => UserEmailConfirmed, x => x.CorrelateById(context =>
            GuidUtils.CreateDeterministicGuid(context.Message.UserId)));

        Event(() => ProfileCreated, x => x.CorrelateById(context =>
            GuidUtils.CreateDeterministicGuid(context.Message.UserId)));

        Event(() => ProfileEmailConfirmed, x => x.CorrelateById(context =>
            GuidUtils.CreateDeterministicGuid(context.Message.UserId)));
    }

    private void ConfigureStateTransitions()
    {
        // Initial state transitions
        Initially(
            When(UserCreated)
                .ThenAsync(HandleUserCreated),

            When(UserEmailConfirmed)
                .Then(HandleUserEmailConfirmed)
                .TransitionTo(ProfileEmailConfirmationPending)
        );

        // ProfileCreationPending state transitions
        During(ProfileCreationPending,
            When(ProfileCreated)
                .ThenAsync(HandleProfileCreated)
                .TransitionTo(ProfileCreationCompleted)
        );

        // ProfileEmailConfirmationPending state transitions
        During(ProfileEmailConfirmationPending,
            When(ProfileEmailConfirmed)
                .Then(HandleProfileEmailConfirmed)
                .TransitionTo(ProfileEmailConfirmationCompleted)
                .Finalize(),

            When(UserCreated)
                .ThenAsync(HandleUserCreated)
        );
    }

    private async Task HandleUserCreated(BehaviorContext<UserRegistrationState, UserCreatedEvent> context)
    {
        _logger.LogInformation(
            "Received {Event} for user {UserId} in Initial state",
            nameof(UserCreatedEvent),
            context.Message.UserId);

        var (userId, email, username, createdAt) = context.Message;

        context.Saga.UserId = userId;
        context.Saga.Email = email;
        context.Saga.Username = username;
        context.Saga.CreatedAt = createdAt;

        await context.Publish(new CreateUserProfileCommand
        {
            UserId = userId,
            Email = email,
            Username = username,
            CreatedAt = createdAt
        });

        await context.TransitionToState(ProfileCreationPending);
    }

    private void HandleUserEmailConfirmed(BehaviorContext<UserRegistrationState, UserEmailConfirmedEvent> context)
    {
        _logger.LogInformation(
            "Received {Event} for user {UserId} in Initial state",
            nameof(UserEmailConfirmedEvent),
            context.Message.UserId);

        context.Saga.UserId = context.Message.UserId;
        context.Saga.Email = context.Message.Email;
        context.Saga.EmailConfirmedAt = context.Message.ConfirmedAt;

        // NOTE: We should not publish the UserEmailConfirmedEvent here, because we want to wait for the ProfileCreatedEvent
    }

    private async Task HandleProfileCreated(BehaviorContext<UserRegistrationState, UserProfileCreatedDomainEvent> context)
    {
        _logger.LogInformation(
            "User profile created for user {UserId}",
            context.Message.UserId);

        if (context.Saga.EmailConfirmedAt != null)
        {
            var confirmedAt = context.Saga.EmailConfirmedAt.Value;

            await context.Publish(new ConfirmUserEmailCommand
            {
                UserId = context.Message.UserId,
                Email = context.Saga.Email,
                ConfirmedAt = confirmedAt
            });

            await context.TransitionToState(ProfileEmailConfirmationPending);
        }
    }

    private void HandleProfileEmailConfirmed(BehaviorContext<UserRegistrationState, UserProfileEmailConfirmedDomainEvent> context)
    {
        _logger.LogInformation(
            "User profile email confirmed for user {UserId}",
            context.Message.UserId);
    }

    // States
    public State ProfileCreationPending { get; private set; } = null!;
    public State ProfileEmailConfirmationPending { get; private set; } = null!;
    public State ProfileEmailConfirmationCompleted { get; private set; } = null!;
    public State ProfileCreationCompleted { get; private set; } = null!;

    // Events
    public Event<UserCreatedEvent> UserCreated { get; private set; } = null!;
    public Event<UserEmailConfirmedEvent> UserEmailConfirmed { get; private set; } = null!;
    public Event<UserProfileCreatedDomainEvent> ProfileCreated { get; private set; } = null!;
    public Event<UserProfileEmailConfirmedDomainEvent> ProfileEmailConfirmed { get; private set; } = null!;
}
