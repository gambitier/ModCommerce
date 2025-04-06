using AccountService.Infrastructure.MessageQueue.IdentityService.Constants;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace AccountService.Infrastructure.Sagas;

public class UserRegistrationStateMachineDefinition : SagaDefinition<UserRegistrationState>
{
    private readonly ILogger<UserRegistrationStateMachineDefinition> _logger;

    public UserRegistrationStateMachineDefinition(ILogger<UserRegistrationStateMachineDefinition> logger)
    {
        _logger = logger;
    }

    protected override void ConfigureSaga(
        IReceiveEndpointConfigurator endpointConfigurator,
        ISagaConfigurator<UserRegistrationState> sagaConfigurator,
        IRegistrationContext context)
    {
        // Configure retry policy, concurrency, etc. if needed
        endpointConfigurator.UseMessageRetry(r => r.Intervals(100, 200, 500, 800, 1000));
        endpointConfigurator.UseInMemoryOutbox(context);

        // Configure message subscriptions
        endpointConfigurator.ConfigureConsumeTopology = false;

        // Configure retry policy
        endpointConfigurator.UseMessageRetry(r =>
            r.Intervals(100, 200));

        endpointConfigurator.ClearSerialization();
        endpointConfigurator.UseRawJsonSerializer();
        endpointConfigurator.UseRawJsonDeserializer(isDefault: true);

        if (endpointConfigurator is IRabbitMqReceiveEndpointConfigurator rabbitMqConfigurator)
        {
            _logger.LogInformation("Binding to exchange: {Exchange}", EventConstants.UserCreatedEvent.Exchange);
            rabbitMqConfigurator.Bind(EventConstants.UserCreatedEvent.Exchange);

            _logger.LogInformation("Binding to exchange: {Exchange}", EventConstants.UserEmailConfirmedEvent.Exchange);
            rabbitMqConfigurator.Bind(EventConstants.UserEmailConfirmedEvent.Exchange);
        }
    }
}