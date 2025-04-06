using AccountService.Infrastructure.Sagas.UserRegistration;
using AccountService.Domain.Events.UserProfile;
using AccountService.Infrastructure.Persistence.Entities;
using AccountService.Domain.Utils;
using MassTransit;

namespace AccountService.Infrastructure.Sagas;

public class UserRegistrationSaga : MassTransitStateMachine<UserRegistrationSagaState>
{
    public required State ProfileCreated { get; set; }
    public required State EmailConfirmed { get; set; }
    public required State Completed { get; set; }
    public required Event<UserAccountCreatedIntegrationEvent> UserAccountCreated { get; set; }
    public required Event<UserEmailConfirmationReceivedIntegrationEvent> UserEmailConfirmationReceived { get; set; }

    public UserRegistrationSaga()
    {
        InstanceState(x => x.CurrentState);

        Event(() => UserAccountCreated, x => x.CorrelateById(context =>
            GuidUtils.CreateDeterministicGuid(context.Message.UserId)));

        Event(() => UserEmailConfirmationReceived, x => x.CorrelateById(context =>
            GuidUtils.CreateDeterministicGuid(context.Message.UserId)));

        Initially(
            When(UserAccountCreated)
                .Then(context => context.Saga.UserId = context.Message.UserId)
                .TransitionTo(ProfileCreated)
                .Publish(context => new CreateUserProfileCommand
                {
                    UserId = context.Message.UserId,
                    Email = context.Message.Email,
                    Username = context.Message.Username,
                    CreatedAt = context.Message.CreatedAt
                })
        );

        During(ProfileCreated,
            When(UserEmailConfirmationReceived)
                .Then(context => context.Saga.Email = context.Message.Email)
                .TransitionTo(EmailConfirmed)
                .Publish(context => new ConfirmUserEmailCommand
                {
                    UserId = context.Saga.UserId,
                    Email = context.Saga.Email,
                    ConfirmedAt = context.Message.ConfirmedAt
                })
                .Finalize()
        );
    }
}
