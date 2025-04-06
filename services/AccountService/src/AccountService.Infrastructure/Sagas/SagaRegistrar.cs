using MassTransit;
using AccountService.Infrastructure.Persistence;

namespace AccountService.Infrastructure.Sagas;

public static class SagaRegistrar
{
    public static void RegisterSagas(this IBusRegistrationConfigurator busConfig)
    {
        busConfig
            .AddSagaStateMachine<UserRegistrationStateMachine, UserRegistrationState>(typeof(UserRegistrationStateMachineDefinition))
            .EntityFrameworkRepository(r =>
            {
                // r.ConcurrencyMode = ConcurrencyMode.Optimistic;
                // r.QueryDelay = TimeSpan.FromSeconds(1);
                r.ExistingDbContext<AccountDbContext>();
                r.UsePostgres();
            });
    }
}
