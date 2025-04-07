using MassTransit;
using Microsoft.Extensions.Logging;
using AccountService.Infrastructure.MessageQueue.IdentityService.Events.UserCreated;
using AccountService.Infrastructure.MessageQueue.IdentityService.Events.UserEmailConfirmed;
using AccountService.Domain.Utils;
using AccountService.Infrastructure.Sagas.Commands;

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

        // Define events
        Event(() => UserCreated, x => x.CorrelateById(context => GuidUtils.CreateDeterministicGuid(context.Message.UserId)));
        Event(() => UserEmailConfirmed, x => x.CorrelateById(context => GuidUtils.CreateDeterministicGuid(context.Message.UserId)));

        // Initially block handles both UserCreated and UserEmailConfirmed as first event
        Initially(
            When(UserCreated)
                .Then(context =>
                {
                    _logger.LogInformation(
                        "Received {Event} for user {UserId} in Initial state",
                        nameof(UserCreatedEvent),
                        context.Message.UserId);

                    context.Saga.UserId = context.Message.UserId;
                    context.Saga.Email = context.Message.Email;
                    context.Saga.Username = context.Message.Username;
                    context.Saga.CreatedAt = context.Message.CreatedAt;
                })
                .PublishAsync(context => context.Init<CreateUserProfileCommand>(new
                {
                    UserId = context.Saga.UserId,
                    Email = context.Saga.Email,
                    Username = context.Saga.Username,
                    CreatedAt = context.Saga.CreatedAt
                }))
                .TransitionTo(UserProfileCreated),

            When(UserEmailConfirmed)
                .Then(context =>
                {
                    _logger.LogInformation(
                        "Received {Event} for user {UserId} in Initial state",
                        nameof(UserEmailConfirmedEvent),
                        context.Message.UserId);

                    context.Saga.UserId = context.Message.UserId;
                    context.Saga.Email = context.Message.Email;
                    context.Saga.EmailConfirmedAt = context.Message.ConfirmedAt;
                })
                .TransitionTo(EmailConfirmed)
        );

        // Handle UserEmailConfirmed when in Created state
        During(UserProfileCreated,
            When(UserEmailConfirmed)
                .Then(context =>
                {
                    _logger.LogInformation(
                        "Received {Event} for user {UserId} in Created state",
                        nameof(UserEmailConfirmedEvent),
                        context.Message.UserId);

                    context.Saga.EmailConfirmedAt = context.Message.ConfirmedAt;
                })
                .PublishAsync(context => context.Init<ConfirmUserEmailCommand>(new
                {
                    UserId = context.Saga.UserId,
                    Email = context.Saga.Email,
                    ConfirmedAt = context.Saga.EmailConfirmedAt
                }))
                .TransitionTo(Completed)
        );

        // Handle UserCreated when in EmailConfirmed state
        During(EmailConfirmed,
            When(UserCreated)
                .Then(context =>
                {
                    _logger.LogInformation(
                        "Received {Event} for user {UserId} in EmailConfirmed state",
                        nameof(UserCreatedEvent),
                        context.Message.UserId);

                    context.Saga.Username = context.Message.Username;
                    context.Saga.CreatedAt = context.Message.CreatedAt;
                })
                .PublishAsync(context => context.Init<CreateUserProfileCommand>(new
                {
                    UserId = context.Saga.UserId,
                    Email = context.Saga.Email,
                    Username = context.Saga.Username,
                    CreatedAt = context.Saga.CreatedAt
                }))
                .PublishAsync(context => context.Init<ConfirmUserEmailCommand>(new
                {
                    UserId = context.Saga.UserId,
                    Email = context.Saga.Email,
                    ConfirmedAt = context.Saga.EmailConfirmedAt
                }))
                .TransitionTo(Completed)
        );

        During(Completed,
            Ignore(UserCreated),
            Ignore(UserEmailConfirmed));

        // Set final state
        SetCompletedWhenFinalized();
    }

    // States
    public State UserProfileCreated { get; private set; } = null!;
    public State EmailConfirmed { get; private set; } = null!;
    public State Completed { get; private set; } = null!;

    // Events
    public Event<UserCreatedEvent> UserCreated { get; private set; } = null!;
    public Event<UserEmailConfirmedEvent> UserEmailConfirmed { get; private set; } = null!;
}
